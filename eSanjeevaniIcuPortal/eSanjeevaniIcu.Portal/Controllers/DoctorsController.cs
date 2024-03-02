using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Portal.Models;
using eSanjeevaniIcu.Shared.Enum;
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
    public class DoctorsController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public DoctorsController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            DoctorsViewModel doctorsViewModel = new DoctorsViewModel();
            try
            {
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                return View(doctorsViewModel);
            }
            return View(doctorsViewModel);
        }
        [HttpGet]
        public ActionResult GetDoctorsList()
        {
            try
            {
                var lstDoctorsViewModel = (from doctors in _context.TbUserDetails
                                           join userMaster in _context.TbUserMasters on doctors.UserId equals userMaster.UserId
                                           join spokeHospital in _context.TbSpokeHospitals on doctors.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                           into allSpokeHospital
                                           from spokeHospital in allSpokeHospital.DefaultIfEmpty()
                                           join districtMaster in _context.TbDistrictMasters on doctors.DistrictId equals districtMaster.DistrictId
                                           into allDistrict
                                           from districtMaster in allDistrict.DefaultIfEmpty()
                                           where doctors.Status == true && userMaster.UserTypeId == (Int32)UserGroupEnum.Doctor
                                           select new DoctorsViewModel
                                           {
                                               DoctorId = doctors.UserDetailId,
                                               SurName = doctors.SurName,
                                               FirstName = doctors.FirstName,
                                               MiddleName = doctors.MiddleName,
                                               LastName = doctors.LastName,
                                               FullName = doctors.SurName + (doctors.FirstName != null ? " " + doctors.FirstName : "") + (doctors.MiddleName != null ? " " + doctors.MiddleName : "") + (doctors.LastName != null ? " " + doctors.LastName : ""),
                                               DistrictId = doctors.DistrictId,
                                               DistrictName = districtMaster.DistrictName,
                                               SpokeHospitalId = doctors.SpokeHospitalId,
                                               SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                               RegistrationNumber = doctors.RegistrationNumber,
                                               ContactNumber = userMaster.MobileNumber,
                                               CreatedOn = doctors.CreatedOn,
                                               Status = doctors.Status,
                                               StatusName = doctors.Status == true ? "Active" : "Inactive",
                                           }).ToList();
                return Json(new { data = lstDoctorsViewModel });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<DoctorsViewModel>() });
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            DoctorsViewModel doctorsViewModel = new DoctorsViewModel();
            try
            {
                doctorsViewModel = (from doctors in _context.TbUserDetails
                                    join userMaster in _context.TbUserMasters on doctors.UserId equals userMaster.UserId
                                    join specialityMaster in _context.TbSpecialityMasters on doctors.SpecialityId equals specialityMaster.SpecialityId
                                    join qualificationMaster in _context.TbQualificationMasters on (Int32)doctors.QualificationId equals qualificationMaster.QualificationId
                                    into allQualification
                                    from qualificationMaster in allQualification.DefaultIfEmpty()
                                    join stateMaster in _context.TbIndianStates on doctors.StateId equals stateMaster.StateId
                                    into allState
                                    from stateMaster in allState.DefaultIfEmpty()
                                    join cityMaster in _context.TbCityMasters on doctors.CityId equals cityMaster.CityId
                                    into allCity
                                    from cityMaster in allCity.DefaultIfEmpty()
                                    join districtMaster in _context.TbDistrictMasters on doctors.DistrictId equals districtMaster.DistrictId
                                    into allDistrict
                                    from districtMaster in allDistrict.DefaultIfEmpty()
                                    join spokeHospital in _context.TbSpokeHospitals on doctors.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                    into allSpokeHospital
                                    from spokeHospital in allSpokeHospital.DefaultIfEmpty()
                                    join createdBy in _context.TbUserMasters on doctors.CreatedBy equals createdBy.UserId
                                    where doctors.Status == true && doctors.UserDetailId == id
                                    select new DoctorsViewModel
                                    {
                                        DoctorId = doctors.UserDetailId,
                                        SurName = doctors.SurName,
                                        FirstName = doctors.FirstName,
                                        MiddleName = doctors.MiddleName,
                                        LastName = doctors.LastName,
                                        Image = doctors.Image,
                                        Signature = doctors.Signature,
                                        FullName = doctors.SurName + (doctors.FirstName != null ? " " + doctors.FirstName : "") + (doctors.MiddleName != null ? " " + doctors.MiddleName : "") + (doctors.LastName != null ? " " + doctors.LastName : ""),
                                        Gender = doctors.Gender,
                                        DateOfBirth = doctors.DateOfBirth,
                                        RegistrationNumber = doctors.RegistrationNumber,
                                        QualificationId = doctors.QualificationId,
                                        QualificationName = qualificationMaster.QualificationName,
                                        SpokeHospitalId = doctors.SpokeHospitalId,
                                        SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                        SpecialityId = doctors.SpecialityId,
                                        SpecialityName = specialityMaster.SpecialityName,
                                        Experience = doctors.Experience,
                                        Language = doctors.Language,
                                        ContactNumber = userMaster.MobileNumber,
                                        EmailAddress = userMaster.Email,
                                        AddressLine1 = doctors.AddressLine1,
                                        AddressLine2 = doctors.AddressLine2,
                                        DistrictId = doctors.DistrictId,
                                        DistrictName = districtMaster.DistrictName,
                                        CityName = cityMaster.CityName,
                                        CityId = doctors.CityId,
                                        StateId = doctors.StateId,
                                        StateName = stateMaster.StateName,
                                        PinCode = doctors.PinCode,
                                        PlaceOfWork = doctors.PlaceOfWork,
                                        FacebookProfile = doctors.FacebookProfile,
                                        TwitterProfile = doctors.TwitterProfile,
                                        LinkedinProfile = doctors.LinkedinProfile,
                                        Status = doctors.Status,
                                        StatusName = doctors.Status == true ? "Active" : "Inactive",
                                        CreatedOn = doctors.CreatedOn,
                                        CreatedByName = createdBy.LastName != null ? (createdBy.FirstName + " " + createdBy.LastName) : createdBy.FirstName,
                                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                doctorsViewModel = new DoctorsViewModel();
                return View(doctorsViewModel);
            }
            return View(doctorsViewModel);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            DoctorsViewModel doctorsViewModel = new DoctorsViewModel();
            try
            {
                doctorsViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                doctorsViewModel.lstSpecialityMaster = _context.TbSpecialityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpecialityId.ToString(), Text = s.SpecialityName }).ToList();
                doctorsViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                doctorsViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                doctorsViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                doctorsViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                doctorsViewModel.lstQualificationMaster = new List<SelectListItem>();
                doctorsViewModel.lstSpecialityMaster = new List<SelectListItem>();
                doctorsViewModel.lstSpokeHospital = new List<SelectListItem>();
                doctorsViewModel.lstStateMaster = new List<SelectListItem>();
                doctorsViewModel.lstDistrictMaster = new List<SelectListItem>();
                doctorsViewModel.lstCityMaster = new List<SelectListItem>();
                return View(doctorsViewModel);
            }
            return View(doctorsViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(DoctorsViewModel doctorsViewModel)
        {
            try
            {
                TbDoctor tbDoctor = new TbDoctor();
                tbDoctor.SurName = doctorsViewModel.SurName;
                tbDoctor.FirstName = doctorsViewModel.FirstName;
                tbDoctor.MiddleName = doctorsViewModel.MiddleName;
                tbDoctor.LastName = doctorsViewModel.LastName;
                tbDoctor.Image = doctorsViewModel.Image;
                tbDoctor.Signature = doctorsViewModel.Signature;
                tbDoctor.Gender = doctorsViewModel.Gender;
                tbDoctor.DateOfBirth = doctorsViewModel.DateOfBirth;
                tbDoctor.RegistrationNumber = doctorsViewModel.RegistrationNumber;
                tbDoctor.QualificationId = doctorsViewModel.QualificationId;
                tbDoctor.SpokeHospitalId = doctorsViewModel.SpokeHospitalId;
                tbDoctor.SpecialityId = doctorsViewModel.SpecialityId;
                tbDoctor.Experience = doctorsViewModel.Experience;
                tbDoctor.Language = doctorsViewModel.Language;
                tbDoctor.ContactNumber = doctorsViewModel.ContactNumber;
                tbDoctor.EmailAddress = doctorsViewModel.EmailAddress;
                tbDoctor.AddressLine1 = doctorsViewModel.AddressLine1;
                tbDoctor.AddressLine2 = doctorsViewModel.AddressLine2;
                tbDoctor.DistrictId = doctorsViewModel.DistrictId;
                tbDoctor.CityId = doctorsViewModel.CityId;
                tbDoctor.StateId = doctorsViewModel.StateId;
                tbDoctor.PinCode = doctorsViewModel.PinCode;
                tbDoctor.PlaceOfWork = doctorsViewModel.PlaceOfWork;
                tbDoctor.FacebookProfile = doctorsViewModel.FacebookProfile;
                tbDoctor.TwitterProfile = doctorsViewModel.TwitterProfile;
                tbDoctor.LinkedinProfile = doctorsViewModel.LinkedinProfile;
                tbDoctor.CreatedOn = DateTime.Now;
                tbDoctor.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                tbDoctor.Status = true;
                _context.Add(tbDoctor);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgSaveSuccess;
            }
            catch (Exception ex)
            {
                doctorsViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                doctorsViewModel.lstSpecialityMaster = _context.TbSpecialityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpecialityId.ToString(), Text = s.SpecialityName }).ToList();
                doctorsViewModel.lstSpokeHospital = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                return View(doctorsViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            DoctorsViewModel doctorsViewModel = new DoctorsViewModel();
            try
            {
                doctorsViewModel = (from doctors in _context.TbDoctors
                                    where doctors.Status == true && doctors.DoctorId == id
                                    select new DoctorsViewModel
                                    {
                                        DoctorId = doctors.DoctorId,
                                        SurName = doctors.SurName,
                                        FirstName = doctors.FirstName,
                                        MiddleName = doctors.MiddleName,
                                        LastName = doctors.LastName,
                                        Gender = doctors.Gender,
                                        Image = doctors.Image,
                                        Signature = doctors.Signature,
                                        DateOfBirth = doctors.DateOfBirth,
                                        RegistrationNumber = doctors.RegistrationNumber,
                                        QualificationId = doctors.QualificationId,
                                        SpokeHospitalId = doctors.SpokeHospitalId,
                                        SpecialityId = doctors.SpecialityId,
                                        Experience = doctors.Experience,
                                        Language = doctors.Language,
                                        ContactNumber = doctors.ContactNumber,
                                        EmailAddress = doctors.EmailAddress,
                                        AddressLine1 = doctors.AddressLine1,
                                        AddressLine2 = doctors.AddressLine2,
                                        DistrictId = doctors.DistrictId,
                                        CityId = doctors.CityId,
                                        StateId = doctors.StateId,
                                        PinCode = doctors.PinCode,
                                        PlaceOfWork = doctors.PlaceOfWork,
                                        FacebookProfile = doctors.FacebookProfile,
                                        TwitterProfile = doctors.TwitterProfile,
                                        LinkedinProfile = doctors.LinkedinProfile,
                                        Status = doctors.Status,
                                    }).FirstOrDefault();
                doctorsViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                doctorsViewModel.lstSpecialityMaster = _context.TbSpecialityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpecialityId.ToString(), Text = s.SpecialityName }).ToList();
                doctorsViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                doctorsViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                doctorsViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                doctorsViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                doctorsViewModel = new DoctorsViewModel();
                doctorsViewModel.lstQualificationMaster = new List<SelectListItem>();
                doctorsViewModel.lstSpecialityMaster = new List<SelectListItem>();
                doctorsViewModel.lstSpokeHospital = new List<SelectListItem>();
                doctorsViewModel.lstStateMaster = new List<SelectListItem>();
                doctorsViewModel.lstDistrictMaster = new List<SelectListItem>();
                doctorsViewModel.lstCityMaster = new List<SelectListItem>();
                return View(doctorsViewModel);
            }
            return View(doctorsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorsViewModel doctorsViewModel)
        {
            try
            {
                TbDoctor tbDoctor = _context.TbDoctors.Find(id);
                tbDoctor.SurName = doctorsViewModel.SurName;
                tbDoctor.FirstName = doctorsViewModel.FirstName;
                tbDoctor.MiddleName = doctorsViewModel.MiddleName;
                tbDoctor.LastName = doctorsViewModel.LastName;
                tbDoctor.Image = doctorsViewModel.Image;
                tbDoctor.Signature = doctorsViewModel.Signature;
                tbDoctor.Gender = doctorsViewModel.Gender;
                tbDoctor.DateOfBirth = doctorsViewModel.DateOfBirth;
                tbDoctor.RegistrationNumber = doctorsViewModel.RegistrationNumber;
                tbDoctor.QualificationId = doctorsViewModel.QualificationId;
                tbDoctor.SpokeHospitalId = doctorsViewModel.SpokeHospitalId;
                tbDoctor.SpecialityId = doctorsViewModel.SpecialityId;
                tbDoctor.Experience = doctorsViewModel.Experience;
                tbDoctor.Language = doctorsViewModel.Language;
                tbDoctor.ContactNumber = doctorsViewModel.ContactNumber;
                tbDoctor.EmailAddress = doctorsViewModel.EmailAddress;
                tbDoctor.AddressLine1 = doctorsViewModel.AddressLine1;
                tbDoctor.AddressLine2 = doctorsViewModel.AddressLine2;
                tbDoctor.DistrictId = doctorsViewModel.DistrictId;
                tbDoctor.CityId = doctorsViewModel.CityId;
                tbDoctor.StateId = doctorsViewModel.StateId;
                tbDoctor.PinCode = doctorsViewModel.PinCode;
                tbDoctor.PlaceOfWork = doctorsViewModel.PlaceOfWork;
                tbDoctor.FacebookProfile = doctorsViewModel.FacebookProfile;
                tbDoctor.TwitterProfile = doctorsViewModel.TwitterProfile;
                tbDoctor.LinkedinProfile = doctorsViewModel.LinkedinProfile;
                tbDoctor.LastUpdatedOn = DateTime.Now;
                tbDoctor.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                _context.Update(tbDoctor);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(doctorsViewModel);
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
