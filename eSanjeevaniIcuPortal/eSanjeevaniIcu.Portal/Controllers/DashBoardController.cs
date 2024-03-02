using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Data.ExtendedEntities;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using eSanjeevaniIcu.Shared.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using eSanjeevaniIcu.Portal.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace eSanjeevaniIcu.Portal.Controllers
{
    [SessionTimeoutAttribute]
    public class DashBoardController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly CommonController _commonController;
        
        private readonly CommonFunctions _commonFunctions;
        private EmailConfiguration EmailConfigurations { get; set; }
        private ApplicationConfigurations ApplicationConfigurations { get; set; }
        public DashBoardController(eSanjeevaniIcuDbContext appcontext, IHubContext<NotificationHub> notificationHub, IOptions<ApplicationConfigurations> settings, IOptions<EmailConfiguration> emailSettings)
        {
            _context = appcontext;
            _notificationHub = notificationHub;
            _commonController = new CommonController(_context, settings, emailSettings);
        }
       
        public IActionResult Index()
        {
            DasboardViewModel dasboardViewModel = new DasboardViewModel();
            List<UserCountEntity> lst = new List<UserCountEntity>();
            int PrincipalId = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
            try
            {
                dasboardViewModel.lstHubHopital = (from hubHospital in _context.TbHubHospitals
                                                   where hubHospital.Status == true
                                                   select new SelectListItem
                                                   {
                                                       Value = hubHospital.HubHospitalId.ToString(),
                                                       Text = hubHospital.HubHospitalName
                                                   }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                return View(dasboardViewModel);
            }
            return View(dasboardViewModel);
        }
        public JsonResult GetPatientDetails(int? HubHospitalId, int? SpokeHospitalId, DateTime? date)
        {
            try
            {
                var patientDetailsGroup = (from pdetails in _context.TbPatientDetails
                                           select new { pdetails.PatientId }).GroupBy(g => new { g.PatientId }).Select(s => s.Key.PatientId).ToList();

                List<DasboardViewModel> lstPatientDetails = new List<DasboardViewModel>();

                foreach (var pdGroup in patientDetailsGroup)
                {
                    var objPatientDetails = (from pdetails in _context.TbPatientDetails
                                             join patient in _context.TbPatients on pdetails.PatientId equals patient.PatientId
                                             join icu in _context.TbIcus on patient.IcuId equals icu.IcuId
                                             join bed in _context.TbBedMasters on patient.BedId equals bed.BedId
                                             join spokeHospital in _context.TbSpokeHospitals on icu.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                             where patient.Status == true && pdetails.PatientId == pdGroup
                                             select new DasboardViewModel
                                             {
                                                 PatientDetailId = pdetails.PatientDetailId,
                                                 PatientId = pdetails.PatientId,
                                                 PatientName = (patient.FirstName != null ? " " + patient.FirstName : "") + (patient.MiddleName != null ? " " + patient.MiddleName : "") + (patient.LastName != null ? " " + patient.LastName : ""),
                                                 PatientCode = "P" + patient.PatientId,
                                                 BedNo = bed.BedNumber,
                                                 IcuNo = icu.IcuName,
                                                 Date = pdetails.Date,
                                                 Temperature = Decimal.Truncate(pdetails.Temperature ?? 0),
                                                 RrRespiratoryRate = Decimal.Truncate(pdetails.RrRespiratoryRate ?? 0),
                                                 OxygenSaturationSpo2 = Decimal.Truncate(pdetails.OxygenSaturationSpo2 ?? 0),
                                                 BloodPressureDia = Decimal.Truncate(pdetails.BloodPressureDia ?? 0),
                                                 BloodPressureSys = Decimal.Truncate(pdetails.BloodPressureSys ?? 0),
                                                 HeartRate = Decimal.Truncate(pdetails.HeartRate ?? 0),
                                                 PatientStatus = pdetails.PatientStatus,
                                                 PatientStatusName = ((PatientStatus)pdetails.PatientStatus).ToString(),
                                                 CreatedOn = pdetails.CreatedOn,
                                                 HubHospitalId = spokeHospital.HubHospitalId,
                                                 SpokeHospitalId = spokeHospital.SpokeHospitalId,
                                                 GenderName = ((GenderName)patient.Gender).ToString(),
                                                 Age = (int)((DateTime.Now - patient.DateOfBirth).TotalDays / 365.242199)
                                             }).OrderByDescending(o => o.CreatedOn).FirstOrDefault();

                    if (objPatientDetails != null)
                    {
                        lstPatientDetails.Add(objPatientDetails);
                    }
                }
                if (HubHospitalId != null)
                {
                    lstPatientDetails = lstPatientDetails.Where(x => x.HubHospitalId == HubHospitalId).ToList();
                }
                if (SpokeHospitalId != null)
                {
                    lstPatientDetails = lstPatientDetails.Where(x => x.SpokeHospitalId == SpokeHospitalId).ToList();
                }
                if (date != null)
                {
                    //lstPatientDetails = lstPatientDetails.Where(x => x.Date.Date == date.Value.Date).ToList();
                }

                return Json(new { success = true, data = lstPatientDetails });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = new List<DasboardViewModel>(), message = ex.Message.ToString() });
            }
        }

        public List<SelectListItem> GetSpokeHospital(int HubHospitalId)
        {
            var list = new List<SelectListItem>();
            list = (from spokeHospital in _context.TbSpokeHospitals
                    where spokeHospital.Status == true && spokeHospital.HubHospitalId == HubHospitalId
                    select new SelectListItem
                    {
                        Value = spokeHospital.SpokeHospitalId.ToString(),
                        Text = spokeHospital.SpokeHospitalName
                    }).ToList();
            return list;
        }
        [HttpPost]
        public IActionResult VideoCall(int UserId, string PatientId, string BedNo)
        {
            //string authToken = "";
            string LoggedInUserName = HttpContext.Session.GetString(Common.LoggedInUserName) ?? "Doctor";
            //_notificationHub.Clients.All.SendAsync("SenderMessage", LoggedInUserName, PatientId);
            string ConnectionId = _commonController.GetSignalRCon(UserId);
            _notificationHub.Clients.Client(ConnectionId).SendAsync("SenderMessage", LoggedInUserName, PatientId);
            return Ok("Success");
        }
        public IActionResult Details(int id)
        {
            DasboardViewModel dasboardViewModel = new DasboardViewModel();
            try
            {
                dasboardViewModel = (from pdetails in _context.TbPatientDetails
                                     join patient in _context.TbPatients on pdetails.PatientId equals patient.PatientId
                                     join icu in _context.TbIcus on patient.IcuId equals icu.IcuId
                                     join bed in _context.TbBedMasters on patient.BedId equals bed.BedId
                                     join spokeHospital in _context.TbSpokeHospitals on icu.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                     //where patient.Status == true && pdetails.PatientId == id && pdetails.Date.Date == DateTime.UtcNow.Date
                                     where patient.Status == true && pdetails.PatientId == id && pdetails.Date.Date == DateTime.Now.Date
                                     select new DasboardViewModel
                                     {
                                         PatientDetailId = pdetails.PatientDetailId,
                                         PatientId = pdetails.PatientId,
                                         PatientName = (patient.FirstName != null ? " " + patient.FirstName : "") + (patient.MiddleName != null ? " " + patient.MiddleName : "") + (patient.LastName != null ? " " + patient.LastName : ""),
                                         PatientCode = "P" + patient.PatientId,
                                         Gender = patient.Gender,
                                         Age = (int)((DateTime.Now - patient.DateOfBirth).TotalDays / 365.242199),
                                         BedNo = bed.BedNumber,
                                         IcuNo = icu.IcuName,
                                         Date = pdetails.Date,
                                         AdmissionDate = patient.AdmitDate,
                                         Temperature = Decimal.Truncate(pdetails.Temperature ?? 0),
                                         RrRespiratoryRate = Decimal.Truncate(pdetails.RrRespiratoryRate ?? 0),
                                         OxygenSaturationSpo2 = Decimal.Truncate(pdetails.OxygenSaturationSpo2 ?? 0),
                                         BloodPressureDia = Decimal.Truncate(pdetails.BloodPressureDia ?? 0),
                                         BloodPressureSys = Decimal.Truncate(pdetails.BloodPressureSys ?? 0),
                                         HeartRate = Decimal.Truncate(pdetails.HeartRate ?? 0),
                                         PatientStatus = pdetails.PatientStatus,
                                         PatientStatusName = ((PatientStatus)pdetails.PatientStatus).ToString(),
                                         CreatedOn = pdetails.CreatedOn,
                                         HubHospitalId = spokeHospital.HubHospitalId,
                                         SpokeHospitalId = spokeHospital.SpokeHospitalId,
                                         SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                         LoggedInUserName = HttpContext.Session.GetString(Common.LoggedInUserName) ?? "Doctor",
                                     }).OrderByDescending(o => o.CreatedOn).FirstOrDefault();
                dasboardViewModel.lstGraphViewModel = (from tbPatientDetails in _context.TbPatientDetails
                                                           //where tbPatientDetails.PatientId == dasboardViewModel.PatientId && tbPatientDetails.Date.Date == DateTime.UtcNow.Date
                                                       where tbPatientDetails.PatientId == dasboardViewModel.PatientId
                                                       select new GraphViewModel
                                                       {
                                                           Date = tbPatientDetails.Date,
                                                           Temperature = Decimal.Truncate(tbPatientDetails.Temperature ?? 0),
                                                           RrRespiratoryRate = Decimal.Truncate(tbPatientDetails.RrRespiratoryRate ?? 0),
                                                           OxygenSaturationSpo2 = Decimal.Truncate(tbPatientDetails.OxygenSaturationSpo2 ?? 0),
                                                           BloodPressureDia = Decimal.Truncate(tbPatientDetails.BloodPressureDia ?? 0),
                                                           BloodPressureSys = Decimal.Truncate(tbPatientDetails.BloodPressureSys ?? 0),
                                                           HeartRate = Decimal.Truncate(tbPatientDetails.HeartRate ?? 0),
                                                           CreatedOn = tbPatientDetails.CreatedOn
                                                       }).OrderBy(o => o.CreatedOn).ToList().TakeLast(20).ToList();
            }
            catch (Exception ex)
            {
                dasboardViewModel = new DasboardViewModel();
                return View(dasboardViewModel);
            }
            return View(dasboardViewModel);
        }
        public List<GraphViewModel> GetGraphValues(int id)
        {
            var lstGraphViewModel = (from tbPatientDetails in _context.TbPatientDetails
                                         //where tbPatientDetails.PatientId == id && tbPatientDetails.Date.Date == DateTime.UtcNow.Date
                                     where tbPatientDetails.PatientId == id
                                     select new GraphViewModel
                                     {
                                         Date = tbPatientDetails.Date,
                                         Temperature = Decimal.Truncate(tbPatientDetails.Temperature ?? 0),
                                         RrRespiratoryRate = Decimal.Truncate(tbPatientDetails.RrRespiratoryRate ?? 0),
                                         OxygenSaturationSpo2 = Decimal.Truncate(tbPatientDetails.OxygenSaturationSpo2 ?? 0),
                                         BloodPressureDia = Decimal.Truncate(tbPatientDetails.BloodPressureDia ?? 0),
                                         BloodPressureSys = Decimal.Truncate(tbPatientDetails.BloodPressureSys ?? 0),
                                         HeartRate = Decimal.Truncate(tbPatientDetails.HeartRate ?? 0),
                                         CreatedOn = tbPatientDetails.CreatedOn
                                     }).OrderBy(o => o.CreatedOn).ToList().TakeLast(20).ToList();
            return lstGraphViewModel;
        }


        public async Task<ActionResult> PatientCommunicationComments(int PatientId, string Comments)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                TbPatientCommunication tbPatientCommunication = null;
                try
                {
                    if (Comments != null && PatientId != 0)
                    {
                        tbPatientCommunication = new TbPatientCommunication();
                        tbPatientCommunication.Comment = Comments;
                        tbPatientCommunication.PatientId = PatientId;
                        tbPatientCommunication.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                        tbPatientCommunication.CreatedOn = DateTime.Now;
                        tbPatientCommunication.Status = true;
                        _context.Add(tbPatientCommunication);
                        await _context.SaveChangesAsync();
                        TempData["ResultMessage"] = "Successful";
                        transaction.Commit();
                        return RedirectToAction("Details", "Dashboard", new { id = PatientId });


                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return View(tbPatientCommunication);
                }
            }
            return RedirectToAction("Details", "Dashboard", new { id = PatientId });

        }

        public JsonResult GetPatientCommunicationComments(int Id)
        {
            PatientCommunicationViewModel patientCommunicationViewModel = new PatientCommunicationViewModel();
            try
            {
                if (Id > 0)
                {
                    patientCommunicationViewModel.lstPatientCommunicationViewModel = (from comments in _context.TbPatientCommunications
                                                                                      join createdBy in _context.TbUserMasters on comments.CreatedBy equals createdBy.UserId into lstCreatedBy
                                                                                      from createdBy in lstCreatedBy.DefaultIfEmpty()
                                                                                      where comments.PatientId == Id
                                                                                      select new PatientCommunicationViewModel
                                                                                      {
                                                                                          Comment = comments.Comment,
                                                                                          CreatedBy = comments.CreatedBy,
                                                                                          CurrentUser = comments.CreatedBy == (HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0) ? true : false,
                                                                                          CreatedByName = createdBy.FirstName + ' ' + createdBy.LastName ?? "",
                                                                                          //CreatedOn = DateTime.ParseExact(comments.CreatedOn.ToString(), "dd/MM/yyyy", null),
                                                                                          CreatedOn = DateTime.Parse(comments.CreatedOn.ToString(), new System.Globalization.CultureInfo("pt-BR")),
                                                                                          /*Convert.ParseExact( comments.CreatedOn.ToString("MM/dd/yyyy hh:mm tt")),*/
                                                                                      }).ToList();
                }
            }
            catch (Exception ex)
            {
                string ms = ex.Message;
                patientCommunicationViewModel.lstPatientCommunicationViewModel = new List<PatientCommunicationViewModel>();
                return Json(patientCommunicationViewModel.lstPatientCommunicationViewModel);
            }
            return Json(patientCommunicationViewModel.lstPatientCommunicationViewModel);
        }
    }
}