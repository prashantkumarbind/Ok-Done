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
    public class SpecialistMasterController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public SpecialistMasterController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            SpecialistMasterViewModel specialistMasterViewModel = new SpecialistMasterViewModel();
            try
            {
                //specialistMasterViewModel.lstSpecialistMasterViewModel = (from specialistMasters in _context.TbSpecialistMasters
                //                                                          join coeMaster in _context.TbCoeMasters on specialistMasters.CoeId equals coeMaster.CoeId
                //                                                          into allcoeMaster
                //                                                          from coeMaster in allcoeMaster.DefaultIfEmpty()
                //                                                          join districtMaster in _context.TbDistrictMasters on specialistMasters.DistrictId equals districtMaster.DistrictId
                //                                                          into allDistrict
                //                                                          from districtMaster in allDistrict.DefaultIfEmpty()
                //                                                          where specialistMasters.Status == true
                //                                                          select new SpecialistMasterViewModel
                //                                                          {
                //                                                              SpecialistId = specialistMasters.SpecialistId,
                //                                                              SurName = specialistMasters.SurName,
                //                                                              FirstName = specialistMasters.FirstName,
                //                                                              MiddleName = specialistMasters.MiddleName,
                //                                                              LastName = specialistMasters.LastName,
                //                                                              FullName = specialistMasters.SurName + (specialistMasters.FirstName != null ? " " + specialistMasters.FirstName : "") + (specialistMasters.MiddleName != null ? " " + specialistMasters.MiddleName : "") + (specialistMasters.LastName != null ? " " + specialistMasters.LastName : ""),
                //                                                              DistrictId = specialistMasters.DistrictId,
                //                                                              DistrictName = districtMaster.DistrictName,
                //                                                              CoeId = specialistMasters.CoeId,
                //                                                              CoeName = coeMaster.CoeName,
                //                                                              RegistrationNumber = specialistMasters.RegistrationNumber,
                //                                                              ContactNumber = specialistMasters.ContactNumber,
                //                                                              CreatedOn = specialistMasters.CreatedOn,
                //                                                              StatusName = specialistMasters.Status == true ? "Active" : "Inactive",
                //                                                          }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                specialistMasterViewModel.lstSpecialistMasterViewModel = new List<SpecialistMasterViewModel>();
                return View(specialistMasterViewModel);
            }
            return View(specialistMasterViewModel);
        }
        [HttpGet]
        public ActionResult GetSpecialistList()
        {
            try
            {
                var lstSpecialistMasterViewModel = (from specialistMasters in _context.TbUserDetails
                                                    join userMaster in _context.TbUserMasters on specialistMasters.UserId equals userMaster.UserId
                                                    join coeMaster in _context.TbCoeMasters on specialistMasters.CoeId equals coeMaster.CoeId
                                                    into allcoeMaster
                                                    from coeMaster in allcoeMaster.DefaultIfEmpty()
                                                    join districtMaster in _context.TbDistrictMasters on specialistMasters.DistrictId equals districtMaster.DistrictId
                                                    into allDistrict
                                                    from districtMaster in allDistrict.DefaultIfEmpty()
                                                    where specialistMasters.Status == true && userMaster.UserTypeId == (Int32)UserGroupEnum.Specialist
                                                    select new SpecialistMasterViewModel
                                                    {
                                                        SpecialistId = specialistMasters.UserDetailId,
                                                        SurName = specialistMasters.SurName,
                                                        FirstName = specialistMasters.FirstName,
                                                        MiddleName = specialistMasters.MiddleName,
                                                        LastName = specialistMasters.LastName,
                                                        FullName = specialistMasters.SurName + (specialistMasters.FirstName != null ? " " + specialistMasters.FirstName : "") + (specialistMasters.MiddleName != null ? " " + specialistMasters.MiddleName : "") + (specialistMasters.LastName != null ? " " + specialistMasters.LastName : ""),
                                                        DistrictId = specialistMasters.DistrictId,
                                                        DistrictName = districtMaster.DistrictName,
                                                        CoeId = specialistMasters.CoeId,
                                                        CoeName = coeMaster.CoeName,
                                                        RegistrationNumber = specialistMasters.RegistrationNumber,
                                                        ContactNumber = userMaster.MobileNumber,
                                                        CreatedOn = specialistMasters.CreatedOn,
                                                        Status = specialistMasters.Status,
                                                        StatusName = specialistMasters.Status == true ? "Active" : "Inactive",
                                                    }).ToList();
                return Json(new { data = lstSpecialistMasterViewModel });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<SpecialistMasterViewModel>() });
            }
        }
        // GET: SpecialistMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            SpecialistMasterViewModel specialistMasterViewModel = new SpecialistMasterViewModel();
            try
            {
                specialistMasterViewModel = (from specialistMasters in _context.TbUserDetails
                                             join userMaster in _context.TbUserMasters on specialistMasters.UserId equals userMaster.UserId
                                             join specialityMaster in _context.TbSpecialityMasters on specialistMasters.SpecialityId equals specialityMaster.SpecialityId
                                             join qualificationMaster in _context.TbQualificationMasters on (Int32)specialistMasters.QualificationId equals qualificationMaster.QualificationId
                                             into allQualification
                                             from qualificationMaster in allQualification.DefaultIfEmpty()
                                             join stateMaster in _context.TbIndianStates on specialistMasters.StateId equals stateMaster.StateId
                                             into allState
                                             from stateMaster in allState.DefaultIfEmpty()
                                             join cityMaster in _context.TbCityMasters on specialistMasters.CityId equals cityMaster.CityId
                                             into allCity
                                             from cityMaster in allCity.DefaultIfEmpty()
                                             join districtMaster in _context.TbDistrictMasters on specialistMasters.DistrictId equals districtMaster.DistrictId
                                             into allDistrict
                                             from districtMaster in allDistrict.DefaultIfEmpty()
                                             join coeMaster in _context.TbCoeMasters on specialistMasters.CoeId equals coeMaster.CoeId
                                             into allcoeMaster
                                             from coeMaster in allcoeMaster.DefaultIfEmpty()
                                             join createdBy in _context.TbUserMasters on specialistMasters.CreatedBy equals createdBy.UserId
                                             where specialistMasters.Status == true && specialistMasters.UserDetailId == id
                                             select new SpecialistMasterViewModel
                                             {
                                                 SpecialistId = specialistMasters.UserDetailId,
                                                 SurName = specialistMasters.SurName,
                                                 FirstName = specialistMasters.FirstName,
                                                 MiddleName = specialistMasters.MiddleName,
                                                 LastName = specialistMasters.LastName,
                                                 Image = specialistMasters.Image,
                                                 Signature = specialistMasters.Signature,
                                                 FullName = specialistMasters.SurName + (specialistMasters.FirstName != null ? " " + specialistMasters.FirstName : "") + (specialistMasters.MiddleName != null ? " " + specialistMasters.MiddleName : "") + (specialistMasters.LastName != null ? " " + specialistMasters.LastName : ""),
                                                 Gender = specialistMasters.Gender,
                                                 DateOfBirth = specialistMasters.DateOfBirth,
                                                 RegistrationNumber = specialistMasters.RegistrationNumber,
                                                 QualificationId = specialistMasters.QualificationId,
                                                 QualificationName = qualificationMaster.QualificationName,
                                                 CoeId = specialistMasters.CoeId,
                                                 CoeName = coeMaster.CoeName,
                                                 SpecialityId = specialistMasters.SpecialityId,
                                                 SpecialityName = specialityMaster.SpecialityName,
                                                 Experience = specialistMasters.Experience,
                                                 Language = specialistMasters.Language,
                                                 ContactNumber = userMaster.MobileNumber,
                                                 EmailAddress = userMaster.Email,
                                                 AddressLine1 = specialistMasters.AddressLine1,
                                                 AddressLine2 = specialistMasters.AddressLine2,
                                                 DistrictId = specialistMasters.DistrictId,
                                                 DistrictName = districtMaster.DistrictName,
                                                 CityName = cityMaster.CityName,
                                                 CityId = specialistMasters.CityId,
                                                 StateId = specialistMasters.StateId,
                                                 StateName = stateMaster.StateName,
                                                 PinCode = specialistMasters.PinCode,
                                                 PlaceOfWork = specialistMasters.PlaceOfWork,
                                                 FacebookProfile = specialistMasters.FacebookProfile,
                                                 TwitterProfile = specialistMasters.TwitterProfile,
                                                 LinkedinProfile = specialistMasters.LinkedinProfile,
                                                 Status = specialistMasters.Status,
                                                 StatusName = specialistMasters.Status == true ? "Active" : "Inactive",
                                                 CreatedOn = specialistMasters.CreatedOn,
                                                 CreatedByName = createdBy.LastName != null ? (createdBy.FirstName + " " + createdBy.LastName) : createdBy.FirstName,
                                             }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                specialistMasterViewModel = new SpecialistMasterViewModel();
                return View(specialistMasterViewModel);
            }
            return View(specialistMasterViewModel);
        }

        // GET: SpecialistMasters/Create
        public IActionResult Create()
        {
            SpecialistMasterViewModel specialistMasterViewModel = new SpecialistMasterViewModel();
            try
            {
                specialistMasterViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                specialistMasterViewModel.lstSpecialityMaster = _context.TbSpecialityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpecialityId.ToString(), Text = s.SpecialityName }).ToList();
                specialistMasterViewModel.lstCoeMaster = _context.TbCoeMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CoeId.ToString(), Text = s.CoeName }).ToList();
                specialistMasterViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                specialistMasterViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                specialistMasterViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                specialistMasterViewModel.lstQualificationMaster = new List<SelectListItem>();
                specialistMasterViewModel.lstSpecialityMaster = new List<SelectListItem>();
                specialistMasterViewModel.lstCoeMaster = new List<SelectListItem>();
                specialistMasterViewModel.lstStateMaster = new List<SelectListItem>();
                specialistMasterViewModel.lstDistrictMaster = new List<SelectListItem>();
                specialistMasterViewModel.lstCityMaster = new List<SelectListItem>();
                return View(specialistMasterViewModel);
            }
            return View(specialistMasterViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SpecialistMasterViewModel specialistMasterViewModel)
        {
            try
            {
                TbSpecialistMaster tbSpecialistMaster = new TbSpecialistMaster();
                tbSpecialistMaster.SurName = specialistMasterViewModel.SurName;
                tbSpecialistMaster.FirstName = specialistMasterViewModel.FirstName;
                tbSpecialistMaster.MiddleName = specialistMasterViewModel.MiddleName;
                tbSpecialistMaster.LastName = specialistMasterViewModel.LastName;
                tbSpecialistMaster.Image = specialistMasterViewModel.Image;
                tbSpecialistMaster.Signature = specialistMasterViewModel.Signature;
                tbSpecialistMaster.Gender = specialistMasterViewModel.Gender;
                tbSpecialistMaster.DateOfBirth = specialistMasterViewModel.DateOfBirth;
                tbSpecialistMaster.RegistrationNumber = specialistMasterViewModel.RegistrationNumber;
                tbSpecialistMaster.QualificationId = specialistMasterViewModel.QualificationId;
                tbSpecialistMaster.CoeId = specialistMasterViewModel.CoeId;
                tbSpecialistMaster.SpecialityId = specialistMasterViewModel.SpecialityId;
                tbSpecialistMaster.Experience = specialistMasterViewModel.Experience;
                tbSpecialistMaster.Language = specialistMasterViewModel.Language;
                tbSpecialistMaster.ContactNumber = specialistMasterViewModel.ContactNumber;
                tbSpecialistMaster.EmailAddress = specialistMasterViewModel.EmailAddress;
                tbSpecialistMaster.AddressLine1 = specialistMasterViewModel.AddressLine1;
                tbSpecialistMaster.AddressLine2 = specialistMasterViewModel.AddressLine2;
                tbSpecialistMaster.DistrictId = specialistMasterViewModel.DistrictId;
                tbSpecialistMaster.CityId = specialistMasterViewModel.CityId;
                tbSpecialistMaster.StateId = specialistMasterViewModel.StateId;
                tbSpecialistMaster.PinCode = specialistMasterViewModel.PinCode;
                tbSpecialistMaster.PlaceOfWork = specialistMasterViewModel.PlaceOfWork;
                tbSpecialistMaster.FacebookProfile = specialistMasterViewModel.FacebookProfile;
                tbSpecialistMaster.TwitterProfile = specialistMasterViewModel.TwitterProfile;
                tbSpecialistMaster.LinkedinProfile = specialistMasterViewModel.LinkedinProfile;
                tbSpecialistMaster.CreatedOn = DateTime.Now;
                tbSpecialistMaster.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                tbSpecialistMaster.Status = true;
                _context.Add(tbSpecialistMaster);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgSaveSuccess;
            }
            catch (Exception ex)
            {
                specialistMasterViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                specialistMasterViewModel.lstSpecialityMaster = _context.TbSpecialityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpecialityId.ToString(), Text = s.SpecialityName }).ToList();
                specialistMasterViewModel.lstCoeMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                return View(specialistMasterViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: SpecialistMasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            SpecialistMasterViewModel specialistMasterViewModel = new SpecialistMasterViewModel();
            try
            {
                specialistMasterViewModel = (from specialistMasters in _context.TbSpecialistMasters
                                             where specialistMasters.Status == true && specialistMasters.SpecialistId == id
                                             select new SpecialistMasterViewModel
                                             {
                                                 SpecialistId = specialistMasters.SpecialistId,
                                                 SurName = specialistMasters.SurName,
                                                 FirstName = specialistMasters.FirstName,
                                                 MiddleName = specialistMasters.MiddleName,
                                                 LastName = specialistMasters.LastName,
                                                 Image = specialistMasters.Image,
                                                 Signature = specialistMasters.Signature,
                                                 Gender = specialistMasters.Gender,
                                                 DateOfBirth = specialistMasters.DateOfBirth,
                                                 RegistrationNumber = specialistMasters.RegistrationNumber,
                                                 QualificationId = specialistMasters.QualificationId,
                                                 CoeId = specialistMasters.CoeId,
                                                 SpecialityId = specialistMasters.SpecialityId,
                                                 Experience = specialistMasters.Experience,
                                                 Language = specialistMasters.Language,
                                                 ContactNumber = specialistMasters.ContactNumber,
                                                 EmailAddress = specialistMasters.EmailAddress,
                                                 AddressLine1 = specialistMasters.AddressLine1,
                                                 AddressLine2 = specialistMasters.AddressLine2,
                                                 DistrictId = specialistMasters.DistrictId,
                                                 CityId = specialistMasters.CityId,
                                                 StateId = specialistMasters.StateId,
                                                 PinCode = specialistMasters.PinCode,
                                                 PlaceOfWork = specialistMasters.PlaceOfWork,
                                                 FacebookProfile = specialistMasters.FacebookProfile,
                                                 TwitterProfile = specialistMasters.TwitterProfile,
                                                 LinkedinProfile = specialistMasters.LinkedinProfile,
                                                 Status = specialistMasters.Status,
                                             }).FirstOrDefault();
                specialistMasterViewModel.lstQualificationMaster = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                specialistMasterViewModel.lstSpecialityMaster = _context.TbSpecialityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpecialityId.ToString(), Text = s.SpecialityName }).ToList();
                specialistMasterViewModel.lstCoeMaster = _context.TbCoeMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CoeId.ToString(), Text = s.CoeName }).ToList();
                specialistMasterViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                specialistMasterViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                specialistMasterViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                specialistMasterViewModel = new SpecialistMasterViewModel();
                specialistMasterViewModel.lstQualificationMaster = new List<SelectListItem>();
                specialistMasterViewModel.lstSpecialityMaster = new List<SelectListItem>();
                specialistMasterViewModel.lstCoeMaster = new List<SelectListItem>();
                specialistMasterViewModel.lstStateMaster = new List<SelectListItem>();
                specialistMasterViewModel.lstDistrictMaster = new List<SelectListItem>();
                specialistMasterViewModel.lstCityMaster = new List<SelectListItem>();
                return View(specialistMasterViewModel);
            }
            return View(specialistMasterViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpecialistMasterViewModel specialistMasterViewModel)
        {
            try
            {
                TbSpecialistMaster tbSpecialistMaster = _context.TbSpecialistMasters.Find(id);
                tbSpecialistMaster.SurName = specialistMasterViewModel.SurName;
                tbSpecialistMaster.FirstName = specialistMasterViewModel.FirstName;
                tbSpecialistMaster.MiddleName = specialistMasterViewModel.MiddleName;
                tbSpecialistMaster.LastName = specialistMasterViewModel.LastName;
                tbSpecialistMaster.Image = specialistMasterViewModel.Image;
                tbSpecialistMaster.Signature = specialistMasterViewModel.Signature;
                tbSpecialistMaster.Gender = specialistMasterViewModel.Gender;
                tbSpecialistMaster.DateOfBirth = specialistMasterViewModel.DateOfBirth;
                tbSpecialistMaster.RegistrationNumber = specialistMasterViewModel.RegistrationNumber;
                tbSpecialistMaster.QualificationId = specialistMasterViewModel.QualificationId;
                tbSpecialistMaster.CoeId = specialistMasterViewModel.CoeId;
                tbSpecialistMaster.SpecialityId = specialistMasterViewModel.SpecialityId;
                tbSpecialistMaster.Experience = specialistMasterViewModel.Experience;
                tbSpecialistMaster.Language = specialistMasterViewModel.Language;
                tbSpecialistMaster.ContactNumber = specialistMasterViewModel.ContactNumber;
                tbSpecialistMaster.EmailAddress = specialistMasterViewModel.EmailAddress;
                tbSpecialistMaster.AddressLine1 = specialistMasterViewModel.AddressLine1;
                tbSpecialistMaster.AddressLine2 = specialistMasterViewModel.AddressLine2;
                tbSpecialistMaster.DistrictId = specialistMasterViewModel.DistrictId;
                tbSpecialistMaster.CityId = specialistMasterViewModel.CityId;
                tbSpecialistMaster.StateId = specialistMasterViewModel.StateId;
                tbSpecialistMaster.PinCode = specialistMasterViewModel.PinCode;
                tbSpecialistMaster.PlaceOfWork = specialistMasterViewModel.PlaceOfWork;
                tbSpecialistMaster.FacebookProfile = specialistMasterViewModel.FacebookProfile;
                tbSpecialistMaster.TwitterProfile = specialistMasterViewModel.TwitterProfile;
                tbSpecialistMaster.LinkedinProfile = specialistMasterViewModel.LinkedinProfile;
                tbSpecialistMaster.LastUpdatedOn = DateTime.Now;
                tbSpecialistMaster.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                _context.Update(tbSpecialistMaster);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(specialistMasterViewModel);
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
