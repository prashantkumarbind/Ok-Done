using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Controllers
{
    [SessionTimeoutAttribute]
    public class PatientController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;
        
        public PatientController(eSanjeevaniIcuDbContext context)
        {
            _context = context;       
          
        }

        public ActionResult Index()
        {
            PatientViewModel patientViewModel = new PatientViewModel();
            try
            {
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientViewModel.lstPatientViewModel = new List<PatientViewModel>();
                return View(patientViewModel);
            }
            return View(patientViewModel);
        }

        [HttpGet]
        public ActionResult GetPatientList()
        {
            try
            {
                var lstPatientViewModel = (from patient in _context.TbPatients
                                           join stateMaster in _context.TbIndianStates on patient.StateId equals stateMaster.StateId
                                           into allState
                                           from stateMaster in allState.DefaultIfEmpty()
                                           join cityMaster in _context.TbCityMasters on patient.CityId equals cityMaster.CityId
                                           into allCity
                                           from cityMaster in allCity.DefaultIfEmpty()
                                           join districtMaster in _context.TbDistrictMasters on patient.DistrictId equals districtMaster.DistrictId
                                           into allDistrict
                                           from districtMaster in allDistrict.DefaultIfEmpty()
                                           join doctor in _context.TbDoctors on patient.DoctorId equals doctor.DoctorId
                                           into allDoctor
                                           from doctor in allDoctor.DefaultIfEmpty()
                                           join bed in _context.TbBedMasters on patient.BedId equals bed.BedId
                                           into allBed
                                           from bed in allBed.DefaultIfEmpty()
                                           join icu in _context.TbIcus on patient.IcuId equals icu.IcuId
                                           into allICU
                                           from icu in allICU.DefaultIfEmpty()
                                           where patient.Status == true
                                           select new PatientViewModel
                                           {
                                               PatientId = patient.PatientId,                                                
                                               FirstName = patient.FirstName,
                                               MiddleName = patient.MiddleName,
                                               LastName = patient.LastName,
                                               FullName = (patient.FirstName != null ? " " + patient.FirstName : "") + (patient.MiddleName != null ? " " + patient.MiddleName : "") + (patient.LastName != null ? " " + patient.LastName : ""),
                                               DoctorId = patient.DoctorId,
                                               DoctorName=doctor.FirstName,
                                               BedId=patient.BedId,
                                               BedName=bed.BedNumber,
                                               IcuId=patient.IcuId,
                                               IcuName=icu.IcuName,
                                               BloodGroup=patient.BloodGroup,
                                               DistrictId = patient.DistrictId,
                                               DistrictName = districtMaster.DistrictName,                                              
                                               ContactNumber = patient.ContactNumber,
                                               CreatedOn = patient.CreatedOn,
                                               Status = patient.Status,
                                               StatusName = patient.Status == true ? "Active" : "Inactive",
                                           }).ToList();
                return Json(new { data = lstPatientViewModel });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<PatientViewModel>() });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            PatientViewModel patientViewModel = new PatientViewModel();
            try
            {
                patientViewModel = (from patient in _context.TbPatients
                                    join stateMaster in _context.TbIndianStates on patient.StateId equals stateMaster.StateId
                                    into allState
                                    from stateMaster in allState.DefaultIfEmpty()
                                    join cityMaster in _context.TbCityMasters on patient.CityId equals cityMaster.CityId
                                    into allCity
                                    from cityMaster in allCity.DefaultIfEmpty()
                                    join districtMaster in _context.TbDistrictMasters on patient.DistrictId equals districtMaster.DistrictId
                                    into allDistrict
                                    from districtMaster in allDistrict.DefaultIfEmpty()
                                    join userMaster in _context.TbUserMasters on patient.CreatedBy equals userMaster.UserId
                                    where patient.Status == true && patient.PatientId == id
                                    select new PatientViewModel
                                    {
                                        PatientId = patient.PatientId,                                       
                                        FirstName = patient.FirstName,
                                        MiddleName = patient.MiddleName,
                                        LastName = patient.LastName,
                                        FullName = (patient.FirstName != null ? " " + patient.FirstName : "") + (patient.MiddleName != null ? " " + patient.MiddleName : "") + (patient.LastName != null ? " " + patient.LastName : ""),
                                        Gender = patient.Gender,
                                        DateOfBirth = patient.DateOfBirth,                                    
                                        ContactNumber = patient.ContactNumber,
                                        Weight = patient.Weight,
                                        BloodGroup = patient.BloodGroup,
                                        AdmitDate = patient.AdmitDate,
                                        IcuId = patient.IcuId,
                                        BedId = patient.BedId,
                                        DoctorId = patient.DoctorId,
                                        Serverity= patient.Serverity,
                                        EmailAddress = patient.EmailAddress,
                                        AddressLine1 = patient.AddressLine1,
                                        AddressLine2 = patient.AddressLine2,
                                        DistrictId = patient.DistrictId,
                                        DistrictName = districtMaster.DistrictName,
                                        CityName = cityMaster.CityName,
                                        CityId = patient.CityId,
                                        StateId = patient.StateId,
                                        StateName = stateMaster.StateName,
                                        PinCode = patient.PinCode,                                       
                                        Status = patient.Status,
                                        StatusName = patient.Status == true ? "Active" : "Inactive",
                                        CreatedOn = patient.CreatedOn,
                                        CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientViewModel = new PatientViewModel();
                return View(patientViewModel);
            }
            return View(patientViewModel);
        }

        // GET: PatientMasters/Create
        public IActionResult Create()
        {
            PatientViewModel patientViewModel = new PatientViewModel();
            try
            {
                patientViewModel.lstICU = _context.TbIcus.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.IcuId.ToString(), Text = s.IcuName }).ToList();
                patientViewModel.lstBed = _context.TbBedMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.BedId.ToString(), Text = s.BedNumber }).ToList();
                patientViewModel.lstDoctor = _context.TbDoctors.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DoctorId.ToString(), Text = (s.FirstName + " "+ s.LastName) }).ToList();
                patientViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                patientViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                patientViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientViewModel.lstICU = new List<SelectListItem>();
                patientViewModel.lstBed = new List<SelectListItem>();
                patientViewModel.lstDoctor = new List<SelectListItem>();
                patientViewModel.lstStateMaster = new List<SelectListItem>();
                patientViewModel.lstDistrictMaster = new List<SelectListItem>();
                patientViewModel.lstCityMaster = new List<SelectListItem>();
                return View(patientViewModel);
            }
            return View(patientViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel patientViewModel)
        {
            try
            {
                TbPatient tbPatient = new TbPatient();               
                tbPatient.FirstName = patientViewModel.FirstName;
                tbPatient.MiddleName = patientViewModel.MiddleName;
                tbPatient.LastName = patientViewModel.LastName;
                tbPatient.DateOfBirth = patientViewModel.DateOfBirth;
                //tbPatient.Age = (int)((DateTime.Now - patientViewModel.DateOfBirth).TotalDays / 365.242199);
                tbPatient.Gender = patientViewModel.Gender;
                tbPatient.ContactNumber = patientViewModel.ContactNumber;
                tbPatient.EmailAddress = patientViewModel.EmailAddress;
                tbPatient.Weight = patientViewModel.Weight;
                tbPatient.BloodGroup = patientViewModel.BloodGroup;
                tbPatient.AdmitDate = patientViewModel.AdmitDate;
                tbPatient.IcuId = patientViewModel.IcuId;
                tbPatient.BedId = patientViewModel.BedId;
                tbPatient.DoctorId = patientViewModel.DoctorId; 
                tbPatient.Serverity= patientViewModel.Serverity;
                tbPatient.AddressLine1 = patientViewModel.AddressLine1;
                tbPatient.AddressLine2 = patientViewModel.AddressLine2;
                tbPatient.DistrictId = patientViewModel.DistrictId;
                tbPatient.CityId = patientViewModel.CityId;
                tbPatient.StateId = patientViewModel.StateId;
                tbPatient.PinCode = patientViewModel.PinCode;               
                tbPatient.CreatedOn = DateTime.Now;
                tbPatient.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                tbPatient.Status = true;
                _context.Add(tbPatient);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgSaveSuccess;
            }
            catch (Exception ex)
            {
                return View(patientViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: PatientMasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            PatientViewModel patientViewModel = new PatientViewModel();
            try
            {
                patientViewModel = (from patient in _context.TbPatients
                                    where patient.Status == true && patient.PatientId == id
                                    select new PatientViewModel
                                    {
                                        PatientId = patient.PatientId,                                      
                                        FirstName = patient.FirstName,
                                        MiddleName = patient.MiddleName,
                                        LastName = patient.LastName,
                                        DateOfBirth = patient.DateOfBirth,
                                        //Age = (int)((DateTime.Now - patient.DateOfBirth).TotalDays / 365.242199),
                                        Gender = patient.Gender,
                                        ContactNumber = patient.ContactNumber,
                                        EmailAddress = patient.EmailAddress,
                                        Weight = patient.Weight,
                                        BloodGroup = patient.BloodGroup,
                                        AdmitDate = patient.AdmitDate,
                                        IcuId = patient.IcuId,
                                        BedId = patient.BedId,
                                        DoctorId = patient.DoctorId,
                                        Serverity= patient.Serverity,
                                        AddressLine1 = patient.AddressLine1,
                                        AddressLine2 = patient.AddressLine2,
                                        DistrictId = patient.DistrictId,
                                        CityId = patient.CityId,
                                        StateId = patient.StateId,
                                        PinCode = patient.PinCode,                                        
                                        Status = patient.Status,
                                    }).FirstOrDefault();
                patientViewModel.lstICU = _context.TbIcus.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.IcuId.ToString(), Text = s.IcuName }).ToList();
                patientViewModel.lstBed = _context.TbBedMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.BedId.ToString(), Text = s.BedNumber }).ToList();
                patientViewModel.lstDoctor = _context.TbDoctors.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DoctorId.ToString(), Text = (s.FirstName + " " + s.LastName) }).ToList();
                patientViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                patientViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                patientViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientViewModel = new PatientViewModel();
                patientViewModel.lstICU = new List<SelectListItem>();
                patientViewModel.lstBed = new List<SelectListItem>();
                patientViewModel.lstDoctor = new List<SelectListItem>();
                patientViewModel.lstStateMaster = new List<SelectListItem>();
                patientViewModel.lstDistrictMaster = new List<SelectListItem>();
                patientViewModel.lstCityMaster = new List<SelectListItem>();
                return View(patientViewModel);
            }
            return View(patientViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientViewModel patientViewModel)
        {
            try
            {
                TbPatient tbPatient = _context.TbPatients.Find(id);                
                tbPatient.FirstName = patientViewModel.FirstName;
                tbPatient.MiddleName = patientViewModel.MiddleName;
                tbPatient.LastName = patientViewModel.LastName;
                tbPatient.Gender = patientViewModel.Gender;
                tbPatient.DateOfBirth = patientViewModel.DateOfBirth;
                //tbPatient.Age = (int)((DateTime.Now - patientViewModel.DateOfBirth).TotalDays / 365.242199);
                tbPatient.ContactNumber = patientViewModel.ContactNumber;
                tbPatient.EmailAddress = patientViewModel.EmailAddress;
                tbPatient.Weight = patientViewModel.Weight;
                tbPatient.BloodGroup = patientViewModel.BloodGroup;
                tbPatient.AdmitDate = patientViewModel.AdmitDate;
                tbPatient.IcuId = patientViewModel.IcuId;
                tbPatient.BedId = patientViewModel.BedId;
                tbPatient.DoctorId = patientViewModel.DoctorId;              
                tbPatient.Serverity= patientViewModel.Serverity;
                tbPatient.AddressLine1 = patientViewModel.AddressLine1;
                tbPatient.AddressLine2 = patientViewModel.AddressLine2;
                tbPatient.DistrictId = patientViewModel.DistrictId;
                tbPatient.CityId = patientViewModel.CityId;
                tbPatient.StateId = patientViewModel.StateId;
                tbPatient.PinCode = patientViewModel.PinCode;
               
                tbPatient.LastUpdatedOn = DateTime.Now;
                tbPatient.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                _context.Update(tbPatient);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(patientViewModel);
            }
            return RedirectToAction(nameof(Index));

        }       

      [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var patient = await _context.TbPatients.FindAsync(id);
                patient.LastUpdatedOn = DateTime.Now;
                patient.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                patient.Status = false;
                _context.Update(patient);
                await _context.SaveChangesAsync();
                return Json(new { Status = true });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false });
            }

        }

        public JsonResult GetIcu()
        {
            var icu = Json(_context.TbIcus.OrderBy(x => x.IcuName).ToList());
            return new JsonResult(icu);
        }

        [HttpPost]
        public JsonResult GetBed(int Id)
        {
            var bed = Json(_context.TbBedMasters.Where(x => x.IcuId == Id).OrderBy(x => x.BedNumber).ToList());
            return new JsonResult(bed);
        }
        
        [HttpPost]
        public JsonResult GetDistrict(int sId)
        {
            var district = Json(_context.TbDistrictMasters.Where(x =>x.StateId==sId).OrderBy(x => x.DistrictName).ToList());
            return new JsonResult(district);
        }
    }
}
