using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Portal.Models;
using eSanjeevaniIcu.Portal.Models.SpokeHospital;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Controllers
{
    [SessionTimeoutAttribute]
    public class SpokeHospitalController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public SpokeHospitalController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }


        public ActionResult Index()
        {
            SpokeHospitalViewModel spokeHospitalViewModel = new SpokeHospitalViewModel();
            try
            {
                
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                spokeHospitalViewModel.lstSpokeHospitalViewModel = new List<SpokeHospitalViewModel>();
                return View(spokeHospitalViewModel);
            }
            return View(spokeHospitalViewModel);
        }

        [HttpGet]
        public ActionResult GetSpokeList()
        {
            try
            {
                var lstSpokeHospitalViewModel = (from spokeHospital in _context.TbSpokeHospitals
                                                 join stateMaster in _context.TbIndianStates on spokeHospital.State equals stateMaster.StateId
                                                    into allState
                                                 from stateMaster in allState.DefaultIfEmpty()
                                                 join coeMaster in _context.TbCoeMasters on spokeHospital.HubHospitalId equals coeMaster.CoeId
                                                 where spokeHospital.Status == true
                                                 select new SpokeHospitalViewModel
                                                 {
                                                     SpokeHospitalId = spokeHospital.SpokeHospitalId,
                                                     SpokeHospitalName = spokeHospital.SpokeHospitalName,                                                     
                                                     HubHospitalName = coeMaster.CoeName,
                                                     ContactNo = spokeHospital.ContactNo,
                                                     Email = spokeHospital.Email,
                                                     City = spokeHospital.City,
                                                     District = spokeHospital.District,
                                                     State = spokeHospital.State,
                                                     StateName = stateMaster.StateName,
                                                     Address1 = spokeHospital.Address1,
                                                     Address2 = spokeHospital.Address2,
                                                     Status = spokeHospital.Status,
                                                     CreatedOn = spokeHospital.CreatedOn,
                                                     StatusName = spokeHospital.Status == true ? "Active" : "Inactive",
                                                 }).ToList();
                return Json(new { data = lstSpokeHospitalViewModel });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<SpokeHospitalViewModel>() });
            }
        }


        [HttpPost]
        public ActionResult AddNurses(int spokeHospitalId)
        {
            MappingViewModel mappingViewModel = new MappingViewModel();
            ViewBag.spokeHospitalId = spokeHospitalId;
            try
            {

                mappingViewModel.lstSpokehospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();

                mappingViewModel.lstNurses = _context.TbNurses.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.NurseId.ToString(), Text = s.SurName + (s.FirstName != null ? " " + s.FirstName : "") + (s.MiddleName != null ? " " + s.MiddleName : "") + (s.LastName != null ? " " + s.LastName : "") }).ToList();
            }
            catch (Exception ex)
            {
                mappingViewModel.lstSpokehospital = new List<SelectListItem>();
                mappingViewModel.lstNurses = new List<SelectListItem>();
                mappingViewModel.lstDoctors = null;
                return View(mappingViewModel);
            }
            return View(mappingViewModel);


        }

        [HttpPost]
        public ActionResult AddDoctors(int spokeHospitalId)
        {
            ViewBag.spokeHospitalId = spokeHospitalId;
            MappingViewModel mappingViewModel = new MappingViewModel();
            try
            {

                mappingViewModel.lstSpokehospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                mappingViewModel.lstDoctors = _context.TbDoctors.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DoctorId.ToString(), Text = s.SurName + (s.FirstName != null ? " " + s.FirstName : "") + (s.MiddleName != null ? " " + s.MiddleName : "") + (s.LastName != null ? " " + s.LastName : "") }).ToList();
                mappingViewModel.lstNurses = null;
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                mappingViewModel.lstSpokehospital = new List<SelectListItem>();
                mappingViewModel.lstDoctors = new List<SelectListItem>();
                mappingViewModel.lstNurses = new List<SelectListItem>();
                return View(mappingViewModel);
            }
            return View(mappingViewModel);


        }
        [HttpPost]
        public ActionResult AddMapping(int mappingKey, int mappingValue, string mappingType)
        {
            try
            {
                var maps = _context.TbMappingMasters.Where(mapobj => mapobj.MappingType.Equals(mappingType) && mapobj.MappingValue.Equals(mappingValue) && mapobj.MappingKey.Equals(mappingKey)).ToList();
                if (maps == null || maps.Count == 0)
                {
                    _context.TbMappingMasters.Add(new TbMappingMaster { MappingKey = mappingKey, MappingValue = mappingValue, MappingType = mappingType });
                    _context.SaveChanges();
                    return Json(new { Status = "Success" });
                }
                else
                {
                    return Json(new { Status = "Fail", Message = "Record already exists" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = "Fail", Message = "Something went wrong" });
            }


        }
        [HttpGet]
        public ActionResult GetMappingDoctors(int key, string type)
        {
            try
            {
                var list = (from mappingList in _context.TbMappingMasters
                            join doctorslist in _context.TbDoctors on mappingList.MappingValue equals doctorslist.DoctorId
                             into allDoctor
                            from doct in allDoctor.DefaultIfEmpty()
                            where mappingList.MappingType.Equals(type) && mappingList.MappingKey.Equals(key)
                            select new MappingModel
                            {
                                MappingKey = mappingList.MappingKey,
                                MappingValue = mappingList.MappingValue,
                                MappingType = mappingList.MappingType,
                                MappingName = doct.SurName + (doct.FirstName != null ? " " + doct.FirstName : "") + (doct.MiddleName != null ? " " + doct.MiddleName : "") + (doct.LastName != null ? " " + doct.LastName : ""),
                            }).ToList(); ;
                return Json(new { data = list });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<MappingModel>() });
            }


        }
        [HttpGet]
        public ActionResult GetMappingNurses(int key, string type)
        {
            try
            {
                var list = (from mappingList in _context.TbMappingMasters
                            join nurse in _context.TbNurses on mappingList.MappingValue equals nurse.NurseId
                             into allNurses
                            from Nurselist in allNurses.DefaultIfEmpty()
                            where mappingList.MappingType.Equals(type) && mappingList.MappingKey.Equals(key)
                            select new MappingModel
                            {
                                MappingKey = mappingList.MappingKey,
                                MappingValue = mappingList.MappingValue,
                                MappingType = mappingList.MappingType,
                                MappingName = Nurselist.SurName + (Nurselist.FirstName != null ? " " + Nurselist.FirstName : "") + (Nurselist.MiddleName != null ? " " + Nurselist.MiddleName : "") + (Nurselist.LastName != null ? " " + Nurselist.LastName : ""),
                                
                            }).ToList(); ;
                return Json(new { data = list });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<MappingModel>() });
            }


        }
        [HttpPost]
        public async Task<ActionResult> UnMap(MappingModel model)
        {
            try
            {
                var list = _context.TbMappingMasters.Where(obj => obj.MappingKey == model.MappingKey && obj.MappingValue == model.MappingValue && obj.MappingType == model.MappingType).ToList();
                if (list.Count > 0)
                {
                    _context.RemoveRange(list);
                    await _context.SaveChangesAsync();
                }

                return Json(new { Status = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Fail" });
            }


        }



        // GET: SpokeHospital/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            SpokeHospitalViewModel spokeHospitalViewModel = new SpokeHospitalViewModel();
            try
            {
                spokeHospitalViewModel = (from spokeHospital in _context.TbSpokeHospitals
                                          join coeMaster in _context.TbCoeMasters on spokeHospital.HubHospitalId equals coeMaster.CoeId
                                          join stateMaster in _context.TbIndianStates on spokeHospital.State equals stateMaster.StateId
                                             into allState
                                          from stateMaster in allState.DefaultIfEmpty()
                                          join cityMaster in _context.TbCityMasters on spokeHospital.City equals cityMaster.CityId
                                          into allCity
                                          from cityMaster in allCity.DefaultIfEmpty()
                                          join districtMaster in _context.TbDistrictMasters on spokeHospital.District equals districtMaster.DistrictId
                                          into allDistrict
                                          from districtMaster in allDistrict.DefaultIfEmpty()
                                          join userMaster in _context.TbUserMasters on spokeHospital.CreatedBy equals userMaster.UserId
                                          where spokeHospital.Status == true && spokeHospital.SpokeHospitalId == id
                                          select new SpokeHospitalViewModel
                                          {
                                              SpokeHospitalId = spokeHospital.SpokeHospitalId,
                                              SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                              HubHospitalName = coeMaster.CoeName,
                                              HubHospitalId = spokeHospital.HubHospitalId,
                                              ContactNo = spokeHospital.ContactNo,
                                              Email = spokeHospital.Email,
                                              Address1 = spokeHospital.Address1,
                                              Address2 = spokeHospital.Address2,
                                              City = spokeHospital.City,
                                              District = spokeHospital.District,
                                              State = spokeHospital.State,
                                              CityName = cityMaster.CityName,
                                              DistrictName = districtMaster.DistrictName,
                                              StateName = stateMaster.StateName,
                                              Pin = spokeHospital.Pin,
                                              Status = spokeHospital.Status,
                                              StatusName = spokeHospital.Status == true ? "Active" : "Inactive",
                                              CreatedOn = spokeHospital.CreatedOn,
                                              CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                spokeHospitalViewModel = new SpokeHospitalViewModel();
                return View(spokeHospitalViewModel);
            }
            return View(spokeHospitalViewModel);
        }
       
        // GET: SpokeHospital/Create
        public IActionResult Create()
        {
            SpokeHospitalViewModel spokeHospitalViewModel = new SpokeHospitalViewModel();
            try
            {
                spokeHospitalViewModel.lstHubHospital = _context.TbCoeMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CoeId.ToString(), Text = s.CoeName }).ToList();
                spokeHospitalViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
                spokeHospitalViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                spokeHospitalViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                spokeHospitalViewModel = new SpokeHospitalViewModel();
                spokeHospitalViewModel.lstCityMaster = new List<SelectListItem>();
                spokeHospitalViewModel.lstDistrictMaster = new List<SelectListItem>();
                spokeHospitalViewModel.lstStateMaster = new List<SelectListItem>();
                return View(spokeHospitalViewModel);
            }
            return View(spokeHospitalViewModel);
        }

        [HttpPost]
        public ActionResult ManageSpoke(int spokeHospitalId)
        {
            ViewBag.spokeHospitalId = spokeHospitalId;


            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(SpokeHospitalViewModel spokeHospitalViewModel)
        {
            try
            {
                TbSpokeHospital spokeHospital = new TbSpokeHospital();
                spokeHospital.SpokeHospitalName = spokeHospitalViewModel.SpokeHospitalName;
                spokeHospital.HubHospitalId = spokeHospitalViewModel.HubHospitalId;
                spokeHospital.ContactNo = spokeHospitalViewModel.ContactNo;
                spokeHospital.Email = spokeHospitalViewModel.Email;
                spokeHospital.Address1 = spokeHospitalViewModel.Address1;
                spokeHospital.Address2 = spokeHospitalViewModel.Address2;
                spokeHospital.City = spokeHospitalViewModel.City;
                spokeHospital.District = spokeHospitalViewModel.District;
                spokeHospital.State = spokeHospitalViewModel.State;
                spokeHospital.Pin = spokeHospitalViewModel.Pin;
                spokeHospital.CreatedOn = DateTime.Now;
                spokeHospital.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                spokeHospital.Status = true;
                _context.Add(spokeHospital);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(spokeHospitalViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: SpokeHospital/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            SpokeHospitalViewModel spokeHospitalViewModel = new SpokeHospitalViewModel();
            try
            {
                spokeHospitalViewModel = (from spokeHospital in _context.TbSpokeHospitals                                         
                                          where spokeHospital.Status == true && spokeHospital.SpokeHospitalId == id
                                          select new SpokeHospitalViewModel
                                          {
                                              SpokeHospitalId = spokeHospital.SpokeHospitalId,
                                              SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                              HubHospitalId = spokeHospital.HubHospitalId,
                                              ContactNo = spokeHospital.ContactNo,
                                              Email = spokeHospital.Email,
                                              Address1 = spokeHospital.Address1,
                                              Address2 = spokeHospital.Address2,                                              
                                              City = spokeHospital.City,
                                              District = spokeHospital.District,
                                              State = spokeHospital.State,
                                              Pin = spokeHospital.Pin,
                                          }).FirstOrDefault();
                spokeHospitalViewModel.lstHubHospital = _context.TbCoeMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CoeId.ToString(), Text = s.CoeName }).ToList();
                spokeHospitalViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                spokeHospitalViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                spokeHospitalViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();

            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                spokeHospitalViewModel = new SpokeHospitalViewModel();
                return View(spokeHospitalViewModel);
            }
            return View(spokeHospitalViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpokeHospitalViewModel spokeHospitalViewModel)
        {
            try
            {
                TbSpokeHospital spokeHospital = _context.TbSpokeHospitals.Find(id);
                spokeHospital.SpokeHospitalName = spokeHospitalViewModel.SpokeHospitalName;
                spokeHospital.HubHospitalId = spokeHospitalViewModel.HubHospitalId;
                spokeHospital.ContactNo = spokeHospitalViewModel.ContactNo;
                spokeHospital.Email = spokeHospitalViewModel.Email;
                spokeHospital.Address1 = spokeHospitalViewModel.Address1;
                spokeHospital.Address2 = spokeHospitalViewModel.Address2;
                spokeHospital.City = spokeHospitalViewModel.City;
                spokeHospital.District = spokeHospitalViewModel.District;
                spokeHospital.State = spokeHospitalViewModel.State;
                spokeHospital.Pin = spokeHospitalViewModel.Pin;
                spokeHospital.LastUpdatedOn = DateTime.Now;
                spokeHospital.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                spokeHospital.Status = true;
                _context.Update(spokeHospital);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(spokeHospitalViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

               
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var spokeHospital = await _context.TbSpokeHospitals.FindAsync(id);
                spokeHospital.LastUpdatedOn = DateTime.Now;
                spokeHospital.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                spokeHospital.Status = false;
                _context.Update(spokeHospital);
                await _context.SaveChangesAsync();
                return Json(new { Status = true });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false });
            }

        }
    }
}
