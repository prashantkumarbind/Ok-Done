using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Models.CoE;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using Microsoft.EntityFrameworkCore.Storage;

namespace eSanjeevaniIcu.Portal.Controllers
{
    public class CoeController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public CoeController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetCoEList()
        {
            try
            {
                var list = (from ceoList in _context.TbCoeMasters
                            join state in _context.TbIndianStates on ceoList.StateId equals state.StateId
                             into allState
                            from state in allState.DefaultIfEmpty()
                            where ceoList.Status == true && ceoList.IsDeleted == false
                            select new CoeModel
                            {
                                CoeId = ceoList.CoeId,
                                CoeName = ceoList.CoeName,
                                FirstName = ceoList.FirstName,
                                MiddleName = ceoList.MiddleName,
                                LastName = ceoList.LastName,
                                //FullName = specialistMasters.SurName + (specialistMasters.FirstName != null ? " " + specialistMasters.FirstName : "") + (specialistMasters.MiddleName != null ? " " + specialistMasters.MiddleName : "") + (specialistMasters.LastName != null ? " " + specialistMasters.LastName : ""),
                                StateId = state.StateId,
                                StateName = state.StateName,
                                ContactNumber = ceoList.ContactNumber,
                                CreatedOn = ceoList.CreatedOn,
                                Status = ceoList.Status ,
                            }).ToList(); ;
                return Json(new { data = list });
            }
            catch(Exception ex)
            {
                return Json(new { data = new List<CoeModel>() });
            }
            
            
        }
       

        [HttpPost]
        public ActionResult AddSpoke(int coeId)
        {
            MappingViewModel mappingViewModel = new MappingViewModel();
            ViewBag.coeId = coeId;
            try
            {

                mappingViewModel.lstCoEMaster = _context.TbCoeMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CoeId.ToString(), Text = s.CoeName }).ToList();
               
                mappingViewModel.lstSpokeMaster = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
            }
            catch (Exception ex)
            {
                mappingViewModel.lstCoEMaster = new List<SelectListItem>();
                mappingViewModel.lstSpokeMaster = new List<SelectListItem>();
                mappingViewModel.lstSpecialistMaster = null;
                return View(mappingViewModel);
            }
            return View(mappingViewModel);


        }

        [HttpPost]
        public ActionResult AddSpecialist(int coeId)
        {
            ViewBag.coeId = coeId;
            MappingViewModel mappingViewModel = new MappingViewModel();
            try
            {

                mappingViewModel.lstCoEMaster = _context.TbCoeMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CoeId.ToString(), Text = s.CoeName }).ToList();
                mappingViewModel.lstSpecialistMaster = _context.TbSpecialistMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpecialistId.ToString(), Text = s.SurName + (s.FirstName != null ? " " + s.FirstName : "") + (s.MiddleName != null ? " " + s.MiddleName : "") + (s.LastName != null ? " " + s.LastName : "") }).ToList();
                mappingViewModel.lstSpokeMaster = null;
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                mappingViewModel.lstCoEMaster = new List<SelectListItem>();
                mappingViewModel.lstSpecialistMaster = new List<SelectListItem>();
                mappingViewModel.lstSpokeMaster = new List<SelectListItem>();
                return View(mappingViewModel);
            }
            return View(mappingViewModel);


        }
        [HttpPost]
        public ActionResult AddMapping(int mappingKey, int mappingValue, string mappingType )
        {
            try
            {
                var maps = _context.TbMappingMasters.Where(mapobj => mapobj.MappingType.Equals(mappingType) && mapobj.MappingValue.Equals(mappingValue) && mapobj.MappingKey.Equals(mappingKey)).ToList();
                if(maps == null || maps.Count == 0)
                {
                    _context.TbMappingMasters.Add(new TbMappingMaster { MappingKey = mappingKey, MappingValue = mappingValue, MappingType = mappingType });
                    _context.SaveChanges();
                    return Json(new { Status = "Success" });
                }
                else
                {
                    return Json(new { Status = "Fail", Message = "Record already exists"});
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Fail" ,Message="Something went wrong"});
            }


        }
        [HttpGet]
        public ActionResult GetMappingSpecialist(int key, string type)
        {
            try
            {
                var list = (from mappingList in _context.TbMappingMasters
                            join specialist in _context.TbSpecialistMasters on mappingList.MappingValue equals specialist.SpecialistId
                             into allSpcl
                            from spcl in allSpcl.DefaultIfEmpty()
                            where mappingList.MappingType.Equals(type) && mappingList.MappingKey.Equals(key)
                            select new MappingModel
                            {
                                MappingKey = mappingList.MappingKey,
                                MappingValue = mappingList.MappingValue,
                                MappingType = mappingList.MappingType,
                                MappingName = spcl.SurName + (spcl.FirstName != null ? " " + spcl.FirstName : "") + (spcl.MiddleName != null ? " " + spcl.MiddleName : "") + (spcl.LastName != null ? " " + spcl.LastName : ""),
                            }).ToList(); ;
                return Json(new { data = list });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<MappingModel>() });
            }


        }
        [HttpGet]
        public ActionResult GetMappingSpoke(int key, string type)
        {
            try
            {
                var list = (from mappingList in _context.TbMappingMasters
                            join spoke in _context.TbSpokeHospitals on mappingList.MappingValue equals spoke.SpokeHospitalId
                             into allSpoke
                            from Spokelist in allSpoke.DefaultIfEmpty()
                            where mappingList.MappingType.Equals(type) && mappingList.MappingKey.Equals(key)
                            select new MappingModel
                            {
                                MappingKey = mappingList.MappingKey,
                                MappingValue = mappingList.MappingValue,
                                MappingType = mappingList.MappingType,
                                MappingName = Spokelist.SpokeHospitalName,
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
                if(list.Count > 0)
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
        public async Task<IActionResult> View(int? id)
        {
            CoeModel CoeViewModel = new CoeModel();
            try
            {
                CoeViewModel = (from ceoList in _context.TbCoeMasters
                               
                                            
                                             join stateMaster in _context.TbIndianStates on ceoList.StateId equals stateMaster.StateId
                                             into allState
                                             from stateMaster in allState.DefaultIfEmpty()
                                             join cityMaster in _context.TbCityMasters on ceoList.CityId equals cityMaster.CityId
                                             into allCity
                                             from cityMaster in allCity.DefaultIfEmpty()
                                             join districtMaster in _context.TbDistrictMasters on ceoList.DistrictId equals districtMaster.DistrictId
                                             into allDistrict
                                             from districtMaster in allDistrict.DefaultIfEmpty()
                                             
                                             where ceoList.Status == true && ceoList.CoeId == id
                                             select new CoeModel
                                             {
                                                 CoeId = ceoList.CoeId,
                                                 Honor = ceoList.Honor,
                                                 FirstName = ceoList.FirstName,
                                                 MiddleName = ceoList.MiddleName,
                                                 LastName = ceoList.LastName,
                                                 FullName = ceoList.Honor + (ceoList.FirstName != null ? " " + ceoList.FirstName : "") + (ceoList.MiddleName != null ? " " + ceoList.MiddleName : "") + (ceoList.LastName != null ? " " + ceoList.LastName : ""),
                                                    Designation = ceoList.Designation,
                                                 CoeName = ceoList.CoeName,
                              
                                                 ContactNumber = ceoList.ContactNumber,
                                                 EmailAddress = ceoList.EmailAddress,
                                                 AddressLine1 = ceoList.AddressLine1,
                                                 AddressLine2 = ceoList.AddressLine2,
                                                 DistrictId = districtMaster.DistrictId,
                                                 DistrictName = districtMaster.DistrictName,
                                                 CityName = cityMaster.CityName,
                                                 CityId = cityMaster.CityId,
                                                 StateId = stateMaster.StateId,
                                                 StateName = stateMaster.StateName,
                                                 PinCode = ceoList.PinCode,
                                                 PlaceOfWork = ceoList.PlaceOfWork,
                                                   StatusName = ceoList.Status?"Active":"InActive",
                                                 Status = ceoList.Status,
                                                 CreatedOn = ceoList.CreatedOn,
                                                
                                             }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                CoeViewModel = new CoeModel();
                return View(CoeViewModel);
            }
            return View(CoeViewModel);
        }
        public ActionResult Create()
        {
           
                CoeModel CoeViewModel = new CoeModel();
                try
                {
                   
                    CoeViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                    CoeViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                    CoeViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
                }
                catch (Exception ex)
                {
                    string message = ex.Message.ToString();
                    CoeViewModel.lstStateMaster = new List<SelectListItem>();
                    CoeViewModel.lstDistrictMaster = new List<SelectListItem>();
                    CoeViewModel.lstCityMaster = new List<SelectListItem>();
                    return View(CoeViewModel);
                }
                return View(CoeViewModel);
            
        }
        [HttpPost]
        public ActionResult ManageCoE(int coeId)
        {
            ViewBag.coeId = coeId; 
            
           
            return View();

        }
        [HttpPost]
    public async Task<IActionResult> Create(CoeModel CoeViewModel)
    {
        try
        {
            TbCoeMaster tbCoeMaster = new TbCoeMaster();
                tbCoeMaster.Honor = CoeViewModel.Honor;
                tbCoeMaster.FirstName = CoeViewModel.FirstName;
                tbCoeMaster.CoeName = CoeViewModel.CoeName;
                tbCoeMaster.CoeCode = CoeViewModel.CoeCode;
            tbCoeMaster.MiddleName = CoeViewModel.MiddleName;
            tbCoeMaster.LastName = CoeViewModel.LastName;
            tbCoeMaster.ContactNumber = CoeViewModel.ContactNumber;
            tbCoeMaster.EmailAddress = CoeViewModel.EmailAddress;
            tbCoeMaster.AddressLine1 = CoeViewModel.AddressLine1;
            tbCoeMaster.AddressLine2 = CoeViewModel.AddressLine2;
            tbCoeMaster.DistrictId = CoeViewModel.DistrictId;
            tbCoeMaster.CityId = CoeViewModel.CityId;
            tbCoeMaster.StateId = CoeViewModel.StateId;
            tbCoeMaster.PinCode = CoeViewModel.PinCode;
            tbCoeMaster.PlaceOfWork = CoeViewModel.PlaceOfWork;
                tbCoeMaster.Designation = CoeViewModel.Designation;
                tbCoeMaster.CreatedOn = DateTime.Now;
            tbCoeMaster.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
            tbCoeMaster.Status = true;
             _context.Add(tbCoeMaster);
            await _context.SaveChangesAsync();
            TempData["SubmitResult"] = Common.MsgUpdateSuccess;
        }
        catch (Exception ex)
        {
                CoeViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                CoeViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                CoeViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
                return View(CoeViewModel);
        }
        return RedirectToAction(nameof(Index));

    }
    public ActionResult Edit(int id)
    {
            CoeModel CoeViewModel = new CoeModel();
            try
            {
                CoeViewModel = (from ceoList in _context.TbCoeMasters
                                where ceoList.Status == true && ceoList.IsDeleted == false && ceoList.CoeId == id
                                             select new CoeModel
                                             {
                                                 Honor = ceoList.Honor,
                                                 CoeId = ceoList.CoeId,
                                                 CoeName = ceoList.CoeName,
                                                 FirstName = ceoList.FirstName,
                                                 MiddleName = ceoList.MiddleName,
                                                 LastName = ceoList.LastName,
                                                 CoeCode = ceoList.CoeCode,
                                                 ContactNumber = ceoList.ContactNumber,
                                                 EmailAddress = ceoList.EmailAddress,
                                                 AddressLine1 = ceoList.AddressLine1,
                                                 AddressLine2 = ceoList.AddressLine2,
                                                 DistrictId = ceoList.DistrictId,
                                                 CityId = ceoList.CityId,
                                                 StateId = ceoList.StateId,
                                                 PinCode = ceoList.PinCode,
                                                 PlaceOfWork = ceoList.PlaceOfWork,
                                                 Designation = ceoList.Designation,
                                                 Status = ceoList.Status,
                                             }).FirstOrDefault();
               
                CoeViewModel.lstStateMaster = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
                CoeViewModel.lstDistrictMaster = _context.TbDistrictMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.DistrictId.ToString(), Text = s.DistrictName }).ToList();
                CoeViewModel.lstCityMaster = _context.TbCityMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.CityId.ToString(), Text = s.CityName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                CoeViewModel = new CoeModel();
                CoeViewModel.lstStateMaster = new List<SelectListItem>();
                CoeViewModel.lstDistrictMaster = new List<SelectListItem>();
                CoeViewModel.lstCityMaster = new List<SelectListItem>();
                return View(CoeViewModel);
            }
            return View(CoeViewModel);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CoeModel CoeViewModel)
        {
            try
            {
                TbCoeMaster tbCoeMaster = _context.TbCoeMasters.Find(id);
                tbCoeMaster.Honor = CoeViewModel.Honor;
                tbCoeMaster.CoeName = CoeViewModel.CoeName;
                tbCoeMaster.CoeCode = CoeViewModel.CoeCode;
                tbCoeMaster.FirstName = CoeViewModel.FirstName;
                tbCoeMaster.MiddleName = CoeViewModel.MiddleName;
                tbCoeMaster.LastName = CoeViewModel.LastName;
                tbCoeMaster.EmailAddress = CoeViewModel.EmailAddress;
                tbCoeMaster.AddressLine1 = CoeViewModel.AddressLine1;
                tbCoeMaster.AddressLine2 = CoeViewModel.AddressLine2;
                tbCoeMaster.DistrictId = CoeViewModel.DistrictId;
                tbCoeMaster.CityId = CoeViewModel.CityId;
                tbCoeMaster.StateId = CoeViewModel.StateId;
                tbCoeMaster.PinCode = CoeViewModel.PinCode;
                tbCoeMaster.PlaceOfWork = CoeViewModel.PlaceOfWork;
                tbCoeMaster.Designation = CoeViewModel.Designation;
                tbCoeMaster.ContactNumber = CoeViewModel.ContactNumber;
                tbCoeMaster.LastUpdatedOn = DateTime.Now;
                tbCoeMaster.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                _context.Update(tbCoeMaster);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(CoeViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var Coe = await _context.TbCoeMasters.FindAsync(id);
                Coe.LastUpdatedOn = DateTime.Now;
                Coe.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                Coe.IsDeleted = true;
                _context.Update(Coe);
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
