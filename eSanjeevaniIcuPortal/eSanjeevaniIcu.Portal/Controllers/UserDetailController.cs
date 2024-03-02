using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SecurityGeneratePwd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Controllers
{
    [SessionTimeoutAttribute]
    public class UserDetailController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public UserDetailController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            UserDetailViewModel userDetailViewModel = new UserDetailViewModel();
            try
            {
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                return View(userDetailViewModel);
            }
            return View(userDetailViewModel);
        }
        [HttpGet]
        public ActionResult GetUserDetailList()
        {
            try
            {
                var lstUserDetailViewModel = (from userDetail in _context.TbUserDetails
                                              join userMaster in _context.TbUserMasters on userDetail.UserId equals userMaster.UserId
                                              join userGroupMaster in _context.TbGroupMasters on userMaster.UserTypeId equals userGroupMaster.GroupId
                                              join districtMaster in _context.TbDistrictMasters on userDetail.DistrictId equals districtMaster.DistrictId
                                              into allDistrict
                                              from districtMaster in allDistrict.DefaultIfEmpty()
                                              where userDetail.Status == true
                                              select new UserDetailViewModel
                                              {
                                                  UserDetailId = userDetail.UserDetailId,
                                                  UserId = userDetail.UserId,
                                                  UserTypeName = userGroupMaster.GroupName,
                                                  SurName = userDetail.SurName,
                                                  FirstName = userDetail.FirstName,
                                                  MiddleName = userDetail.MiddleName,
                                                  LastName = userDetail.LastName,
                                                  FullName = userDetail.SurName + (userDetail.FirstName != null ? " " + userDetail.FirstName : "") + (userDetail.MiddleName != null ? " " + userDetail.MiddleName : "") + (userDetail.LastName != null ? " " + userDetail.LastName : ""),
                                                  DistrictId = userDetail.DistrictId,
                                                  DistrictName = districtMaster.DistrictName,
                                                  CoeId = userDetail.CoeId,
                                                  RegistrationNumber = userDetail.RegistrationNumber,
                                                  ContactNumber = userMaster.MobileNumber,
                                                  CreatedOn = userDetail.CreatedOn,
                                                  Status = userDetail.Status,
                                                  StatusName = userDetail.Status == true ? "Active" : "Inactive",
                                              }).ToList();
                return Json(new { data = lstUserDetailViewModel });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<UserDetailViewModel>() });
            }
        }
        // GET: MenuMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            UserDetailViewModel userDetailViewModel = new UserDetailViewModel();
            try
            {
                userDetailViewModel = (from userDetail in _context.TbUserDetails
                                       join user in _context.TbUserMasters on userDetail.UserId equals user.UserId
                                       join userGroupMaster in _context.TbGroupMasters on user.UserTypeId equals userGroupMaster.GroupId
                                       join specialityMaster in _context.TbSpecialityMasters on userDetail.SpecialityId equals specialityMaster.SpecialityId
                                       into allSpecialityMaster
                                       from specialityMaster in allSpecialityMaster.DefaultIfEmpty()
                                       join spokeHospital in _context.TbSpokeHospitals on userDetail.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                       into allspokeHospital
                                       from spokeHospital in allspokeHospital.DefaultIfEmpty()
                                       join qualificationMaster in _context.TbQualificationMasters on (Int32)userDetail.QualificationId equals qualificationMaster.QualificationId
                                       into allQualification
                                       from qualificationMaster in allQualification.DefaultIfEmpty()
                                       join stateMaster in _context.TbIndianStates on userDetail.StateId equals stateMaster.StateId
                                       into allState
                                       from stateMaster in allState.DefaultIfEmpty()
                                       join cityMaster in _context.TbCityMasters on userDetail.CityId equals cityMaster.CityId
                                       into allCity
                                       from cityMaster in allCity.DefaultIfEmpty()
                                       join districtMaster in _context.TbDistrictMasters on userDetail.DistrictId equals districtMaster.DistrictId
                                       into allDistrict
                                       from districtMaster in allDistrict.DefaultIfEmpty()
                                       join coeMaster in _context.TbCoeMasters on userDetail.CoeId equals coeMaster.CoeId
                                       into allcoeMaster
                                       from coeMaster in allcoeMaster.DefaultIfEmpty()
                                       join userMaster in _context.TbUserMasters on userDetail.CreatedBy equals userMaster.UserId
                                       where userDetail.Status == true && userDetail.UserDetailId == id
                                       select new UserDetailViewModel
                                       {
                                           UserDetailId = userDetail.UserDetailId,
                                           SurName = userDetail.SurName,
                                           UserTypeName = userGroupMaster.GroupName,
                                           FirstName = userDetail.FirstName,
                                           MiddleName = userDetail.MiddleName,
                                           LastName = userDetail.LastName,
                                           Image = userDetail.Image,
                                           Signature = userDetail.Signature,
                                           FullName = userDetail.SurName + (userDetail.FirstName != null ? " " + userDetail.FirstName : "") + (userDetail.MiddleName != null ? " " + userDetail.MiddleName : "") + (userDetail.LastName != null ? " " + userDetail.LastName : ""),
                                           Gender = userDetail.Gender,
                                           DateOfBirth = userDetail.DateOfBirth,
                                           RegistrationNumber = userDetail.RegistrationNumber,
                                           QualificationId = userDetail.QualificationId,
                                           QualificationName = qualificationMaster.QualificationName,
                                           CoeId = userDetail.CoeId,
                                           CoeName = coeMaster.CoeName,
                                           SpokeHospitalId = userDetail.SpokeHospitalId,
                                           SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                           SpecialityId = userDetail.SpecialityId,
                                           SpecialityName = specialityMaster.SpecialityName,
                                           Experience = userDetail.Experience,
                                           Language = userDetail.Language,
                                           ContactNumber = user.MobileNumber,
                                           EmailAddress = user.Email,
                                           AddressLine1 = userDetail.AddressLine1,
                                           AddressLine2 = userDetail.AddressLine2,
                                           DistrictId = userDetail.DistrictId,
                                           DistrictName = districtMaster.DistrictName,
                                           CityName = cityMaster.CityName,
                                           CityId = userDetail.CityId,
                                           StateId = userDetail.StateId,
                                           StateName = stateMaster.StateName,
                                           PinCode = userDetail.PinCode,
                                           PlaceOfWork = userDetail.PlaceOfWork,
                                           FacebookProfile = userDetail.FacebookProfile,
                                           TwitterProfile = userDetail.TwitterProfile,
                                           LinkedinProfile = userDetail.LinkedinProfile,
                                           Status = userDetail.Status,
                                           StatusName = userDetail.Status == true ? "Active" : "Inactive",
                                           CreatedOn = userDetail.CreatedOn,
                                           CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                       }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                userDetailViewModel = new UserDetailViewModel();
                return View(userDetailViewModel);
            }
            return View(userDetailViewModel);
        }

        // GET: MenuMasters/Create
        public IActionResult Create(string refPage = "")
        {
            UserDetailViewModel userDetailViewModel = new UserDetailViewModel();
            try
            {
                userDetailViewModel.lstUserType = _context.TbGroupMasters.Where(x => x.Status == true && x.GroupId != 1 && x.GroupId != 2).Select(s => new SelectListItem { Value = s.GroupId.ToString(), Text = s.GroupName }).ToList();
                userDetailViewModel.refPage = refPage;
                if (refPage != "")
                {
                    userDetailViewModel.UserTypeId = Convert.ToInt32(userDetailViewModel.lstUserType.Where(x => x.Text.ToLower().Contains(refPage.ToLower())).Select(s => s.Value).FirstOrDefault());
                }
                userDetailViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                userDetailViewModel.lstSpecialityMaster = _context.TbSpecialityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpecialityId.ToString(), Text = s.SpecialityName }).ToList();
                userDetailViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                userDetailViewModel.lstCoeMaster = _context.TbCoeMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CoeId.ToString(), Text = s.CoeName }).ToList();
                userDetailViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                userDetailViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                userDetailViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                userDetailViewModel.refPage = refPage;
                userDetailViewModel.lstQualificationMaster = new List<SelectListItem>();
                userDetailViewModel.lstSpecialityMaster = new List<SelectListItem>();
                userDetailViewModel.lstSpokeHospital = new List<SelectListItem>();
                userDetailViewModel.lstCoeMaster = new List<SelectListItem>();
                userDetailViewModel.lstStateMaster = new List<SelectListItem>();
                userDetailViewModel.lstDistrictMaster = new List<SelectListItem>();
                userDetailViewModel.lstCityMaster = new List<SelectListItem>();
                userDetailViewModel.lstUserType = new List<SelectListItem>();
                return View(userDetailViewModel);
            }
            return View(userDetailViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserDetailViewModel userDetailViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    TbUserMaster tbUserMaster = new TbUserMaster();
                    tbUserMaster.FirstName = userDetailViewModel.FirstName;
                    tbUserMaster.LastName = userDetailViewModel.LastName;
                    tbUserMaster.MobileNumber = userDetailViewModel.ContactNumber;
                    tbUserMaster.Email = userDetailViewModel.EmailAddress;
                    tbUserMaster.UserName = userDetailViewModel.FirstName + (userDetailViewModel.LastName != null && userDetailViewModel.LastName != "" ? userDetailViewModel.LastName : "");
                    tbUserMaster.Password = RandomPasswordGenerator.encode("pass1234");
                    tbUserMaster.UserTypeId = userDetailViewModel.UserTypeId;
                    tbUserMaster.CreatedOn = DateTime.Now;
                    tbUserMaster.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    tbUserMaster.Status = true;
                    _context.Add(tbUserMaster);
                    await _context.SaveChangesAsync();

                    TbUserDetail tbUserDetail = new TbUserDetail();
                    tbUserDetail.UserId = tbUserMaster.UserId;
                    tbUserDetail.SurName = userDetailViewModel.SurName;
                    tbUserDetail.FirstName = userDetailViewModel.FirstName;
                    tbUserDetail.MiddleName = userDetailViewModel.MiddleName;
                    tbUserDetail.LastName = userDetailViewModel.LastName;
                    tbUserDetail.Image = userDetailViewModel.Image;
                    tbUserDetail.Signature = userDetailViewModel.Signature;
                    tbUserDetail.Gender = userDetailViewModel.Gender;
                    tbUserDetail.DateOfBirth = userDetailViewModel.DateOfBirth;
                    tbUserDetail.RegistrationNumber = userDetailViewModel.RegistrationNumber;
                    tbUserDetail.QualificationId = userDetailViewModel.QualificationId;
                    tbUserDetail.CoeId = userDetailViewModel.CoeId;
                    tbUserDetail.SpecialityId = userDetailViewModel.SpecialityId;
                    tbUserDetail.SpokeHospitalId = userDetailViewModel.SpokeHospitalId;
                    tbUserDetail.Experience = userDetailViewModel.Experience;
                    tbUserDetail.Language = userDetailViewModel.Language;
                    tbUserDetail.AddressLine1 = userDetailViewModel.AddressLine1;
                    tbUserDetail.AddressLine2 = userDetailViewModel.AddressLine2;
                    tbUserDetail.DistrictId = userDetailViewModel.DistrictId;
                    tbUserDetail.CityId = userDetailViewModel.CityId;
                    tbUserDetail.StateId = userDetailViewModel.StateId;
                    tbUserDetail.PinCode = userDetailViewModel.PinCode;
                    tbUserDetail.PlaceOfWork = userDetailViewModel.PlaceOfWork;
                    tbUserDetail.FacebookProfile = userDetailViewModel.FacebookProfile;
                    tbUserDetail.TwitterProfile = userDetailViewModel.TwitterProfile;
                    tbUserDetail.LinkedinProfile = userDetailViewModel.LinkedinProfile;
                    tbUserDetail.CreatedOn = DateTime.Now;
                    tbUserDetail.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    tbUserDetail.Status = true;
                    _context.Add(tbUserDetail);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    TempData["SubmitResult"] = Common.MsgSaveSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    userDetailViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                    userDetailViewModel.lstSpecialityMaster = _context.TbSpecialityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpecialityId.ToString(), Text = s.SpecialityName }).ToList();
                    userDetailViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                    userDetailViewModel.lstCoeMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                    userDetailViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                    userDetailViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                    userDetailViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
                    userDetailViewModel.lstUserType = _context.TbGroupMasters.Where(x => x.Status == true && x.GroupId != 1 && x.GroupId != 2).Select(s => new SelectListItem { Value = s.GroupId.ToString(), Text = s.GroupName }).ToList();
                    return View(userDetailViewModel);
                }
                if (!string.IsNullOrWhiteSpace(userDetailViewModel.refPage))
                {
                    if (userDetailViewModel.refPage.ToLower() == "doctor")
                    {
                        return RedirectToAction("Index", "Doctors");
                    }
                    else if (userDetailViewModel.refPage.ToLower() == "nurse")
                    {
                        return RedirectToAction("Index", "Nurse");
                    }
                    else if (userDetailViewModel.refPage.ToLower() == "specialist")
                    {
                        return RedirectToAction("Index", "SpecialistMaster");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: MenuMasters/Edit/5
        public async Task<IActionResult> Edit(int? id, string refPage = "")
        {
            UserDetailViewModel userDetailViewModel = new UserDetailViewModel();
            try
            {
                userDetailViewModel = (from userDetail in _context.TbUserDetails
                                       join userMaster in _context.TbUserMasters on userDetail.UserId equals userMaster.UserId
                                       where userDetail.Status == true && userDetail.UserDetailId == id
                                       select new UserDetailViewModel
                                       {
                                           UserDetailId = userDetail.UserDetailId,
                                           UserTypeId = userMaster.UserTypeId,
                                           UserId = userDetail.UserId,
                                           SurName = userDetail.SurName,
                                           FirstName = userDetail.FirstName,
                                           MiddleName = userDetail.MiddleName,
                                           LastName = userDetail.LastName,
                                           Image = userDetail.Image,
                                           Signature = userDetail.Signature,
                                           Gender = userDetail.Gender,
                                           DateOfBirth = userDetail.DateOfBirth,
                                           RegistrationNumber = userDetail.RegistrationNumber,
                                           QualificationId = userDetail.QualificationId,
                                           CoeId = userDetail.CoeId,
                                           SpecialityId = userDetail.SpecialityId,
                                           SpokeHospitalId = userDetail.SpokeHospitalId,
                                           Experience = userDetail.Experience,
                                           Language = userDetail.Language,
                                           ContactNumber = userMaster.MobileNumber,
                                           EmailAddress = userMaster.Email,
                                           AddressLine1 = userDetail.AddressLine1,
                                           AddressLine2 = userDetail.AddressLine2,
                                           DistrictId = userDetail.DistrictId,
                                           CityId = userDetail.CityId,
                                           StateId = userDetail.StateId,
                                           PinCode = userDetail.PinCode,
                                           PlaceOfWork = userDetail.PlaceOfWork,
                                           FacebookProfile = userDetail.FacebookProfile,
                                           TwitterProfile = userDetail.TwitterProfile,
                                           LinkedinProfile = userDetail.LinkedinProfile,
                                           Status = userDetail.Status,
                                       }).FirstOrDefault();
                userDetailViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                userDetailViewModel.lstSpecialityMaster = _context.TbSpecialityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpecialityId.ToString(), Text = s.SpecialityName }).ToList();
                userDetailViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                userDetailViewModel.lstCoeMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                userDetailViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                userDetailViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                userDetailViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
                userDetailViewModel.lstUserType = _context.TbGroupMasters.Where(x => x.Status == true && x.GroupId != 1 && x.GroupId != 2).Select(s => new SelectListItem { Value = s.GroupId.ToString(), Text = s.GroupName }).ToList();
                userDetailViewModel.refPage = refPage;
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                userDetailViewModel = new UserDetailViewModel();
                userDetailViewModel.refPage = refPage;
                userDetailViewModel.lstQualificationMaster = new List<SelectListItem>();
                userDetailViewModel.lstSpecialityMaster = new List<SelectListItem>();
                userDetailViewModel.lstSpokeHospital = new List<SelectListItem>();
                userDetailViewModel.lstCoeMaster = new List<SelectListItem>();
                userDetailViewModel.lstStateMaster = new List<SelectListItem>();
                userDetailViewModel.lstDistrictMaster = new List<SelectListItem>();
                userDetailViewModel.lstCityMaster = new List<SelectListItem>();
                userDetailViewModel.lstUserType = new List<SelectListItem>();
                return View(userDetailViewModel);
            }
            return View(userDetailViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserDetailViewModel userDetailViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    TbUserMaster tbUserMaster = _context.TbUserMasters.Find(userDetailViewModel.UserId);
                    tbUserMaster.FirstName = userDetailViewModel.FirstName;
                    tbUserMaster.LastName = userDetailViewModel.LastName;
                    tbUserMaster.MobileNumber = userDetailViewModel.ContactNumber;
                    tbUserMaster.Email = userDetailViewModel.EmailAddress;
                    tbUserMaster.UserName = userDetailViewModel.FirstName + (userDetailViewModel.LastName != null && userDetailViewModel.LastName != "" ? userDetailViewModel.LastName : "");
                    tbUserMaster.Password = RandomPasswordGenerator.encode("pass1234");
                    tbUserMaster.UserTypeId = userDetailViewModel.UserTypeId;
                    tbUserMaster.LastUpdatedOn = DateTime.Now;
                    tbUserMaster.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    _context.Update(tbUserMaster);
                    await _context.SaveChangesAsync();

                    TbUserDetail tbUserDetail = _context.TbUserDetails.Find(userDetailViewModel.UserDetailId);
                    tbUserDetail.UserId = tbUserMaster.UserId;
                    tbUserDetail.SurName = userDetailViewModel.SurName;
                    tbUserDetail.FirstName = userDetailViewModel.FirstName;
                    tbUserDetail.MiddleName = userDetailViewModel.MiddleName;
                    tbUserDetail.LastName = userDetailViewModel.LastName;
                    tbUserDetail.Image = userDetailViewModel.Image;
                    tbUserDetail.Signature = userDetailViewModel.Signature;
                    tbUserDetail.Gender = userDetailViewModel.Gender;
                    tbUserDetail.DateOfBirth = userDetailViewModel.DateOfBirth;
                    tbUserDetail.RegistrationNumber = userDetailViewModel.RegistrationNumber;
                    tbUserDetail.QualificationId = userDetailViewModel.QualificationId;
                    tbUserDetail.CoeId = userDetailViewModel.CoeId;
                    tbUserDetail.SpecialityId = userDetailViewModel.SpecialityId;
                    tbUserDetail.SpokeHospitalId = userDetailViewModel.SpokeHospitalId;
                    tbUserDetail.Experience = userDetailViewModel.Experience;
                    tbUserDetail.Language = userDetailViewModel.Language;
                    tbUserDetail.AddressLine1 = userDetailViewModel.AddressLine1;
                    tbUserDetail.AddressLine2 = userDetailViewModel.AddressLine2;
                    tbUserDetail.DistrictId = userDetailViewModel.DistrictId;
                    tbUserDetail.CityId = userDetailViewModel.CityId;
                    tbUserDetail.StateId = userDetailViewModel.StateId;
                    tbUserDetail.PinCode = userDetailViewModel.PinCode;
                    tbUserDetail.PlaceOfWork = userDetailViewModel.PlaceOfWork;
                    tbUserDetail.FacebookProfile = userDetailViewModel.FacebookProfile;
                    tbUserDetail.TwitterProfile = userDetailViewModel.TwitterProfile;
                    tbUserDetail.LinkedinProfile = userDetailViewModel.LinkedinProfile;
                    tbUserDetail.LastUpdatedOn = DateTime.Now;
                    tbUserDetail.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    _context.Update(tbUserDetail);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    TempData["SubmitResult"] = Common.MsgUpdateSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return View(userDetailViewModel);
                }
                if (!string.IsNullOrWhiteSpace(userDetailViewModel.refPage))
                {
                    if (userDetailViewModel.refPage.ToLower() == "doctor")
                    {
                        return RedirectToAction("Index", "Doctors");
                    }
                    else if (userDetailViewModel.refPage.ToLower() == "nurse")
                    {
                        return RedirectToAction("Index", "Nurse");
                    }
                    else if (userDetailViewModel.refPage.ToLower() == "specialist")
                    {
                        return RedirectToAction("Index", "SpecialistMaster");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: MenuMasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserDetailViewModel userDetailViewModel = new UserDetailViewModel();
            try
            {
                userDetailViewModel = (from userDetail in _context.TbUserDetails
                                       join user in _context.TbUserMasters on userDetail.UserId equals user.UserId
                                       join userGroupMaster in _context.TbGroupMasters on user.UserTypeId equals userGroupMaster.GroupId
                                       join specialityMaster in _context.TbSpecialityMasters on userDetail.SpecialityId equals specialityMaster.SpecialityId
                                       into allSpecialityMaster
                                       from specialityMaster in allSpecialityMaster.DefaultIfEmpty()
                                       join spokeHospital in _context.TbSpokeHospitals on userDetail.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                       into allspokeHospital
                                       from spokeHospital in allspokeHospital.DefaultIfEmpty()
                                       join qualificationMaster in _context.TbQualificationMasters on (Int32)userDetail.QualificationId equals qualificationMaster.QualificationId
                                       into allQualification
                                       from qualificationMaster in allQualification.DefaultIfEmpty()
                                       join stateMaster in _context.TbIndianStates on userDetail.StateId equals stateMaster.StateId
                                       into allState
                                       from stateMaster in allState.DefaultIfEmpty()
                                       join cityMaster in _context.TbCityMasters on userDetail.CityId equals cityMaster.CityId
                                       into allCity
                                       from cityMaster in allCity.DefaultIfEmpty()
                                       join districtMaster in _context.TbDistrictMasters on userDetail.DistrictId equals districtMaster.DistrictId
                                       into allDistrict
                                       from districtMaster in allDistrict.DefaultIfEmpty()
                                       join coeMaster in _context.TbCoeMasters on userDetail.CoeId equals coeMaster.CoeId
                                       into allcoeMaster
                                       from coeMaster in allcoeMaster.DefaultIfEmpty()
                                       join userMaster in _context.TbUserMasters on userDetail.CreatedBy equals userMaster.UserId
                                       where userDetail.Status == true && userDetail.UserDetailId == id
                                       select new UserDetailViewModel
                                       {
                                           UserDetailId = userDetail.UserDetailId,
                                           SurName = userDetail.SurName,
                                           UserTypeName = userGroupMaster.GroupName,
                                           FirstName = userDetail.FirstName,
                                           MiddleName = userDetail.MiddleName,
                                           LastName = userDetail.LastName,
                                           Image = userDetail.Image,
                                           Signature = userDetail.Signature,
                                           FullName = userDetail.SurName + (userDetail.FirstName != null ? " " + userDetail.FirstName : "") + (userDetail.MiddleName != null ? " " + userDetail.MiddleName : "") + (userDetail.LastName != null ? " " + userDetail.LastName : ""),
                                           Gender = userDetail.Gender,
                                           DateOfBirth = userDetail.DateOfBirth,
                                           RegistrationNumber = userDetail.RegistrationNumber,
                                           QualificationId = userDetail.QualificationId,
                                           QualificationName = qualificationMaster.QualificationName,
                                           CoeId = userDetail.CoeId,
                                           CoeName = coeMaster.CoeName,
                                           SpokeHospitalId = userDetail.SpokeHospitalId,
                                           SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                           SpecialityId = userDetail.SpecialityId,
                                           SpecialityName = specialityMaster.SpecialityName,
                                           Experience = userDetail.Experience,
                                           Language = userDetail.Language,
                                           ContactNumber = user.MobileNumber,
                                           EmailAddress = user.Email,
                                           AddressLine1 = userDetail.AddressLine1,
                                           AddressLine2 = userDetail.AddressLine2,
                                           DistrictId = userDetail.DistrictId,
                                           DistrictName = districtMaster.DistrictName,
                                           CityName = cityMaster.CityName,
                                           CityId = userDetail.CityId,
                                           StateId = userDetail.StateId,
                                           StateName = stateMaster.StateName,
                                           PinCode = userDetail.PinCode,
                                           PlaceOfWork = userDetail.PlaceOfWork,
                                           FacebookProfile = userDetail.FacebookProfile,
                                           TwitterProfile = userDetail.TwitterProfile,
                                           LinkedinProfile = userDetail.LinkedinProfile,
                                           Status = userDetail.Status,
                                           StatusName = userDetail.Status == true ? "Active" : "Inactive",
                                           CreatedOn = userDetail.CreatedOn,
                                           CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                       }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                userDetailViewModel = new UserDetailViewModel();
                return View(userDetailViewModel);
            }
            return View(userDetailViewModel);
        }

        // POST: MenuMasters/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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

                    TempData["SubmitResult"] = Common.MsgDeleteSuccess;
                    transaction.Commit();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    TempData["SubmitResult"] = Common.MsgError;
                    transaction.Rollback();
                }
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
