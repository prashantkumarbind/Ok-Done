using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Portal.Models;
using eSanjeevaniIcu.Portal.Models.CoE;
using eSanjeevaniIcu.Shared.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class NUrseController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public NUrseController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            NurseViewModel nurseViewModel = new NurseViewModel();
            try
            {
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                nurseViewModel.lstNurseViewModel = new List<NurseViewModel>();
                return View(nurseViewModel);
            }
            return View(nurseViewModel);
        }

        [HttpGet]
        public ActionResult GetNurseList()
        {
            try
            {
                var lstNurseViewModel = (from nurse in _context.TbUserDetails
                                         join userMaster in _context.TbUserMasters on nurse.UserId equals userMaster.UserId
                                         join spokeHospital in _context.TbSpokeHospitals on nurse.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                         into allSpokeHospital
                                         from spokeHospital in allSpokeHospital.DefaultIfEmpty()
                                         join districtMaster in _context.TbDistrictMasters on nurse.DistrictId equals districtMaster.DistrictId
                                         into allDistrict
                                         from districtMaster in allDistrict.DefaultIfEmpty()
                                         where nurse.Status == true && userMaster.UserTypeId == (Int32)UserGroupEnum.Nurse
                                         select new NurseViewModel
                                         {
                                             NurseId = nurse.UserDetailId,
                                             SurName = nurse.SurName,
                                             FirstName = nurse.FirstName,
                                             MiddleName = nurse.MiddleName,
                                             LastName = nurse.LastName,
                                             FullName = nurse.SurName + (nurse.FirstName != null ? " " + nurse.FirstName : "") + (nurse.MiddleName != null ? " " + nurse.MiddleName : "") + (nurse.LastName != null ? " " + nurse.LastName : ""),
                                             DistrictId = nurse.DistrictId,
                                             DistrictName = districtMaster.DistrictName,
                                             SpokeHospitalId = nurse.SpokeHospitalId,
                                             SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                             RegistrationNumber = nurse.RegistrationNumber,
                                             ContactNumber = userMaster.MobileNumber,
                                             CreatedOn = nurse.CreatedOn,
                                             Status = nurse.Status,
                                             StatusName = nurse.Status == true ? "Active" : "Inactive",
                                         }).ToList();
                return Json(new { data = lstNurseViewModel });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<NurseViewModel>() });
            }
        }

        // GET: Nurse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            NurseViewModel nurseViewModel = new NurseViewModel();
            try
            {
                nurseViewModel = (from nurse in _context.TbUserDetails
                                  join userMaster in _context.TbUserMasters on nurse.UserId equals userMaster.UserId
                                  join qualificationMaster in _context.TbQualificationMasters on (Int32)nurse.QualificationId equals qualificationMaster.QualificationId
                                  into allQualification
                                  from qualificationMaster in allQualification.DefaultIfEmpty()
                                  join stateMaster in _context.TbIndianStates on nurse.StateId equals stateMaster.StateId
                                  into allState
                                  from stateMaster in allState.DefaultIfEmpty()
                                  join cityMaster in _context.TbCityMasters on nurse.CityId equals cityMaster.CityId
                                  into allCity
                                  from cityMaster in allCity.DefaultIfEmpty()
                                  join districtMaster in _context.TbDistrictMasters on nurse.DistrictId equals districtMaster.DistrictId
                                  into allDistrict
                                  from districtMaster in allDistrict.DefaultIfEmpty()
                                  join spokeHospital in _context.TbSpokeHospitals on nurse.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                  into allSpokeHospital
                                  from spokeHospital in allSpokeHospital.DefaultIfEmpty()
                                  join createdBy in _context.TbUserMasters on nurse.CreatedBy equals createdBy.UserId
                                  where nurse.Status == true && nurse.UserDetailId == id
                                  select new NurseViewModel
                                  {
                                      NurseId = nurse.UserDetailId,
                                      SurName = nurse.SurName,
                                      FirstName = nurse.FirstName,
                                      MiddleName = nurse.MiddleName,
                                      LastName = nurse.LastName,
                                      Image = nurse.Image,
                                      Signature = nurse.Signature,
                                      FullName = nurse.SurName + (nurse.FirstName != null ? " " + nurse.FirstName : "") + (nurse.MiddleName != null ? " " + nurse.MiddleName : "") + (nurse.LastName != null ? " " + nurse.LastName : ""),
                                      Gender = nurse.Gender,
                                      DateOfBirth = nurse.DateOfBirth,
                                      RegistrationNumber = nurse.RegistrationNumber,
                                      QualificationId = nurse.QualificationId,
                                      QualificationName = qualificationMaster.QualificationName,
                                      SpokeHospitalId = nurse.SpokeHospitalId,
                                      SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                      Experience = nurse.Experience,
                                      Language = nurse.Language,
                                      ContactNumber = userMaster.MobileNumber,
                                      EmailAddress = userMaster.Email,
                                      AddressLine1 = nurse.AddressLine1,
                                      AddressLine2 = nurse.AddressLine2,
                                      DistrictId = nurse.DistrictId,
                                      DistrictName = districtMaster.DistrictName,
                                      CityName = cityMaster.CityName,
                                      CityId = nurse.CityId,
                                      StateId = nurse.StateId,
                                      StateName = stateMaster.StateName,
                                      PinCode = nurse.PinCode,
                                      FacebookProfile = nurse.FacebookProfile,
                                      TwitterProfile = nurse.TwitterProfile,
                                      LinkedinProfile = nurse.LinkedinProfile,
                                      Status = nurse.Status,
                                      StatusName = nurse.Status == true ? "Active" : "Inactive",
                                      CreatedOn = nurse.CreatedOn,
                                      CreatedByName = createdBy.LastName != null ? (createdBy.FirstName + " " + createdBy.LastName) : createdBy.FirstName,
                                  }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                nurseViewModel = new NurseViewModel();
                return View(nurseViewModel);
            }
            return View(nurseViewModel);
        }

        // GET: Nurse/Create
        public IActionResult Create()
        {
            NurseViewModel nurseViewModel = new NurseViewModel();
            try
            {
                nurseViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                nurseViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                nurseViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                nurseViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                nurseViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                nurseViewModel.lstQualificationMaster = new List<SelectListItem>();
                nurseViewModel.lstSpokeHospital = new List<SelectListItem>();
                nurseViewModel.lstStateMaster = new List<SelectListItem>();
                nurseViewModel.lstDistrictMaster = new List<SelectListItem>();
                nurseViewModel.lstCityMaster = new List<SelectListItem>();
                return View(nurseViewModel);
            }
            return View(nurseViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(NurseViewModel nurseViewModel)
        {
            try
            {
                TbNurse tbNurse = new TbNurse();
                tbNurse.SurName = nurseViewModel.SurName;
                tbNurse.FirstName = nurseViewModel.FirstName;
                tbNurse.MiddleName = nurseViewModel.MiddleName;
                tbNurse.LastName = nurseViewModel.LastName;
                tbNurse.Image = nurseViewModel.Image;
                tbNurse.Signature = nurseViewModel.Signature;
                tbNurse.Gender = nurseViewModel.Gender;
                tbNurse.DateOfBirth = nurseViewModel.DateOfBirth;
                tbNurse.RegistrationNumber = nurseViewModel.RegistrationNumber;
                tbNurse.QualificationId = nurseViewModel.QualificationId;
                tbNurse.SpokeHospitalId = nurseViewModel.SpokeHospitalId;
                tbNurse.Experience = nurseViewModel.Experience;
                tbNurse.Language = nurseViewModel.Language;
                tbNurse.ContactNumber = nurseViewModel.ContactNumber;
                tbNurse.EmailAddress = nurseViewModel.EmailAddress;
                tbNurse.AddressLine1 = nurseViewModel.AddressLine1;
                tbNurse.AddressLine2 = nurseViewModel.AddressLine2;
                tbNurse.DistrictId = nurseViewModel.DistrictId;
                tbNurse.CityId = nurseViewModel.CityId;
                tbNurse.StateId = nurseViewModel.StateId;
                tbNurse.PinCode = nurseViewModel.PinCode;
                tbNurse.FacebookProfile = nurseViewModel.FacebookProfile;
                tbNurse.TwitterProfile = nurseViewModel.TwitterProfile;
                tbNurse.LinkedinProfile = nurseViewModel.LinkedinProfile;
                tbNurse.CreatedOn = DateTime.Now;
                tbNurse.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                tbNurse.Status = true;
                _context.Add(tbNurse);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgSaveSuccess;
            }
            catch (Exception ex)
            {
                nurseViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                nurseViewModel.lstSpokeHospital = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                return View(nurseViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Nurse/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            NurseViewModel nurseViewModel = new NurseViewModel();
            try
            {
                nurseViewModel = (from nurse in _context.TbNurses
                                  where nurse.Status == true && nurse.NurseId == id
                                  select new NurseViewModel
                                  {
                                      NurseId = nurse.NurseId,
                                      SurName = nurse.SurName,
                                      FirstName = nurse.FirstName,
                                      MiddleName = nurse.MiddleName,
                                      LastName = nurse.LastName,
                                      Image = nurse.Image,
                                      Signature = nurse.Signature,
                                      Gender = nurse.Gender,
                                      DateOfBirth = nurse.DateOfBirth,
                                      RegistrationNumber = nurse.RegistrationNumber,
                                      QualificationId = nurse.QualificationId,
                                      SpokeHospitalId = nurse.SpokeHospitalId,
                                      Experience = nurse.Experience,
                                      Language = nurse.Language,
                                      ContactNumber = nurse.ContactNumber,
                                      EmailAddress = nurse.EmailAddress,
                                      AddressLine1 = nurse.AddressLine1,
                                      AddressLine2 = nurse.AddressLine2,
                                      DistrictId = nurse.DistrictId,
                                      CityId = nurse.CityId,
                                      StateId = nurse.StateId,
                                      PinCode = nurse.PinCode,
                                      FacebookProfile = nurse.FacebookProfile,
                                      TwitterProfile = nurse.TwitterProfile,
                                      LinkedinProfile = nurse.LinkedinProfile,
                                      Status = nurse.Status,
                                  }).FirstOrDefault();
                nurseViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                nurseViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                nurseViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                nurseViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                nurseViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                nurseViewModel = new NurseViewModel();
                nurseViewModel.lstQualificationMaster = new List<SelectListItem>();
                nurseViewModel.lstSpokeHospital = new List<SelectListItem>();
                nurseViewModel.lstStateMaster = new List<SelectListItem>();
                nurseViewModel.lstDistrictMaster = new List<SelectListItem>();
                nurseViewModel.lstCityMaster = new List<SelectListItem>();
                return View(nurseViewModel);
            }
            return View(nurseViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NurseViewModel nurseViewModel)
        {
            try
            {
                TbNurse tbNurse = _context.TbNurses.Find(id);
                tbNurse.SurName = nurseViewModel.SurName;
                tbNurse.FirstName = nurseViewModel.FirstName;
                tbNurse.MiddleName = nurseViewModel.MiddleName;
                tbNurse.LastName = nurseViewModel.LastName;
                tbNurse.Gender = nurseViewModel.Gender;
                tbNurse.Image = nurseViewModel.Image;
                tbNurse.Signature = nurseViewModel.Signature;
                tbNurse.DateOfBirth = nurseViewModel.DateOfBirth;
                tbNurse.RegistrationNumber = nurseViewModel.RegistrationNumber;
                tbNurse.QualificationId = nurseViewModel.QualificationId;
                tbNurse.SpokeHospitalId = nurseViewModel.SpokeHospitalId;
                tbNurse.Experience = nurseViewModel.Experience;
                tbNurse.Language = nurseViewModel.Language;
                tbNurse.ContactNumber = nurseViewModel.ContactNumber;
                tbNurse.EmailAddress = nurseViewModel.EmailAddress;
                tbNurse.AddressLine1 = nurseViewModel.AddressLine1;
                tbNurse.AddressLine2 = nurseViewModel.AddressLine2;
                tbNurse.DistrictId = nurseViewModel.DistrictId;
                tbNurse.CityId = nurseViewModel.CityId;
                tbNurse.StateId = nurseViewModel.StateId;
                tbNurse.PinCode = nurseViewModel.PinCode;
                tbNurse.FacebookProfile = nurseViewModel.FacebookProfile;
                tbNurse.TwitterProfile = nurseViewModel.TwitterProfile;
                tbNurse.LinkedinProfile = nurseViewModel.LinkedinProfile;
                tbNurse.LastUpdatedOn = DateTime.Now;
                tbNurse.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                _context.Update(tbNurse);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(nurseViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userDetail = await _context.TbUserDetails.FindAsync(id);
                    userDetail.LastUpdatedOn = DateTime.Now;
                    userDetail.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    userDetail.Status = false;
                    _context.Update(userDetail);
                    await _context.SaveChangesAsync();

                    var userMaster = await _context.TbUserMasters.FindAsync(userDetail.UserId);
                    userMaster.LastUpdatedOn = DateTime.Now;
                    userMaster.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    userMaster.Status = false;
                    _context.Update(userMaster);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return Json(new { Status = true });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { Status = false });
                }
            }
        }
    }
}
