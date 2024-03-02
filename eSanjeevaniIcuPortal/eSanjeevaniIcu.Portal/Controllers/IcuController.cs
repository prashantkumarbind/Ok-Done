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
    public class IcuController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public IcuController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            IcuViewModel icuViewModel = new IcuViewModel();
            try
            {
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                return View(icuViewModel);
            }
            return View(icuViewModel);
        }
        [HttpGet]
        public ActionResult GetIcuList()
        {
            try
            {
                var lstIcuViewModel = (from icu in _context.TbIcus
                                       join spokeHospital in _context.TbSpokeHospitals on icu.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                       into allSpokeHospital
                                       from spokeHospital in allSpokeHospital.DefaultIfEmpty()
                                       join stateMas in _context.TbStates on icu.StateId equals stateMas.StateId
                                       into allState
                                       from stateMas in allState.DefaultIfEmpty()
                                       where icu.Status == true
                                       select new IcuViewModel
                                       {
                                           IcuId = icu.IcuId,
                                           IcuName = icu.IcuName,
                                           StateId = icu.StateId,
                                           StateName = stateMas.StateName,
                                           SpokeHospitalId = icu.SpokeHospitalId,
                                           SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                           TotalBeds = icu.TotalBeds,
                                           HduBeds = icu.HduBeds,
                                           IcuBeds = icu.IcuBeds,
                                           Other = icu.Other,
                                           Beds = icu.Beds ?? 0,
                                           CreatedOn = icu.CreatedOn,
                                           Status = icu.Status,
                                           StatusName = icu.Status == true ? "Active" : "Inactive",
                                       }).ToList();
                return Json(new { data = lstIcuViewModel });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<IcuViewModel>() });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            IcuViewModel icuViewModel = new IcuViewModel();
            try
            {
                icuViewModel = (from icu in _context.TbIcus
                                join stateMaster in _context.TbStates on icu.StateId equals stateMaster.StateId
                                into allState
                                from stateMaster in allState.DefaultIfEmpty()
                                join spokeHospital in _context.TbSpokeHospitals on icu.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                into allSpokeHospital
                                from spokeHospital in allSpokeHospital.DefaultIfEmpty()
                                join userMaster in _context.TbUserMasters on icu.CreatedBy equals userMaster.UserId
                                where icu.Status == true && icu.IcuId == id
                                select new IcuViewModel
                                {
                                    IcuId = icu.IcuId,
                                    IcuName = icu.IcuName,
                                    StateId = icu.StateId,
                                    StateName = stateMaster.StateName,
                                    SpokeHospitalId = icu.SpokeHospitalId,
                                    SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                    TotalBeds = icu.TotalBeds,
                                    HduBeds = icu.HduBeds,
                                    IcuBeds = icu.IcuBeds,
                                    Other = icu.Other,
                                    Beds = icu.Beds ?? 0,
                                    Status = icu.Status,
                                    StatusName = icu.Status == true ? "Active" : "Inactive",
                                    CreatedOn = icu.CreatedOn,
                                    CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                icuViewModel = new IcuViewModel();
                return View(icuViewModel);
            }
            return View(icuViewModel);
        }

        // GET: Icu/Create
        public IActionResult Create()
        {
            IcuViewModel icuViewModel = new IcuViewModel();
            try
            {
                icuViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                icuViewModel.lstStateMaster = _context.TbStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                icuViewModel.lstSpokeHospital = new List<SelectListItem>();
                icuViewModel.lstStateMaster = new List<SelectListItem>();
                return View(icuViewModel);
            }
            return View(icuViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create(IcuViewModel icuViewModel)
        {
            try
            {
                TbIcu tbIcu = new TbIcu();
                tbIcu.IcuName = icuViewModel.IcuName;
                tbIcu.StateId = icuViewModel.StateId;
                tbIcu.SpokeHospitalId = icuViewModel.SpokeHospitalId;
                tbIcu.TotalBeds = icuViewModel.TotalBeds;
                tbIcu.HduBeds = icuViewModel.HduBeds;
                tbIcu.IcuBeds = icuViewModel.IcuBeds;
                tbIcu.Other = icuViewModel.Other;
                tbIcu.Beds = icuViewModel.Beds ?? 0;
                tbIcu.CreatedOn = DateTime.Now;
                tbIcu.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                tbIcu.Status = true;
                _context.Add(tbIcu);
                await _context.SaveChangesAsync();


                // Get the newly generated ICUId
                int newIcuId = tbIcu.IcuId;
                int currentBedNumber = 1;

                // Create ICU bed entries
                for (int i = 1; i <= icuViewModel.IcuBeds; i++)
                {
                    TbBedMaster icuBed = new TbBedMaster();
                    icuBed.IcuId = newIcuId;
                    icuBed.SpokeHospitalId = icuViewModel.SpokeHospitalId;
                    icuBed.BedType = "I";
                    icuBed.BedNumber = "BD" + currentBedNumber.ToString("000");
                    icuBed.CreatedOn = DateTime.Now;
                    icuBed.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    icuBed.Status = true;

                    _context.Add(icuBed);
                    currentBedNumber++;
                }

                // Create HDU bed entries
                for (int i = 1; i <= icuViewModel.HduBeds; i++)
                {
                    TbBedMaster hduBed = new TbBedMaster();
                    hduBed.IcuId = newIcuId;
                    hduBed.SpokeHospitalId = icuViewModel.SpokeHospitalId;
                    hduBed.BedType = "H";
                    hduBed.BedNumber = "BD" + currentBedNumber.ToString("000");
                    hduBed.CreatedOn = DateTime.Now;
                    hduBed.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    hduBed.Status = true;

                    _context.Add(hduBed);
                    currentBedNumber++;
                }

                // Create Other bed entries
                for (int i = 1; i <= icuViewModel.Beds; i++)
                {
                    TbBedMaster otherBed = new TbBedMaster();
                    otherBed.IcuId = newIcuId;
                    otherBed.SpokeHospitalId = icuViewModel.SpokeHospitalId;
                    otherBed.BedType = "O";
                    otherBed.BedNumber = "BD" + currentBedNumber.ToString("000");
                    otherBed.CreatedOn = DateTime.Now;
                    otherBed.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    otherBed.Status = true;

                    _context.Add(otherBed);
                    currentBedNumber++;
                }
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgSaveSuccess;
            }
            catch (Exception ex)
            {
                icuViewModel.lstSpokeHospital = _context.TbQualificationMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.QualificationId.ToString(), Text = s.QualificationName }).ToList();
                return View(icuViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult GetSpokeHospitals(int stateId)
        {
            var spokeHospitals = _context.TbSpokeHospitals.Where(spokeHospital => spokeHospital.State == stateId)
                .Select(s => new SelectListItem
                {
                    Value = s.SpokeHospitalId.ToString(),
                    Text = s.SpokeHospitalName
                }).ToList();

            if (spokeHospitals.Count == 0)
            {
                spokeHospitals.Add(new SelectListItem { Value = "0", Text = "No Spoke Hospitals available" });
            }
            return Json(spokeHospitals);
        }


        // GET: Icu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            IcuViewModel icuViewModel = new IcuViewModel();
            try
            {
                icuViewModel = (from icu in _context.TbIcus
                                join stateMaster in _context.TbStates on icu.StateId equals stateMaster.StateId
                                into allState
                                from stateMaster in allState.DefaultIfEmpty()
                                join spokeHospital in _context.TbSpokeHospitals on icu.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                into allSpokeHospital
                                from spokeHospital in allSpokeHospital.DefaultIfEmpty()
                                join userMaster in _context.TbUserMasters on icu.CreatedBy equals userMaster.UserId
                                where icu.Status == true && icu.IcuId == id
                                select new IcuViewModel
                                {
                                    IcuId = icu.IcuId,
                                    IcuName = icu.IcuName,
                                    StateId = icu.StateId,
                                    StateName = stateMaster.StateName,
                                    SpokeHospitalId = icu.SpokeHospitalId,
                                    SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                    TotalBeds = icu.TotalBeds,
                                    HduBeds = icu.HduBeds,
                                    IcuBeds = icu.IcuBeds,
                                    Other = icu.Other,
                                    Beds = icu.Beds ?? 0,
                                    Status = icu.Status,

                                }).FirstOrDefault();
                icuViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                icuViewModel.lstStateMaster = _context.TbStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                icuViewModel = new IcuViewModel();
                icuViewModel.lstSpokeHospital = new List<SelectListItem>();
                icuViewModel.lstStateMaster = new List<SelectListItem>();
                return View(icuViewModel);
            }
            return View(icuViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IcuViewModel icuViewModel)
        {
            try
            {
                TbIcu tbIcu = _context.TbIcus.Find(id);
                tbIcu.IcuName = icuViewModel.IcuName;
                tbIcu.StateId = icuViewModel.StateId;
                tbIcu.SpokeHospitalId = icuViewModel.SpokeHospitalId;
                tbIcu.TotalBeds = icuViewModel.TotalBeds;
                tbIcu.HduBeds = icuViewModel.HduBeds;
                tbIcu.IcuBeds = icuViewModel.IcuBeds;
                tbIcu.Other = icuViewModel.Other;
                tbIcu.Beds = icuViewModel.Beds ?? 0;

                tbIcu.LastUpdatedOn = DateTime.Now;
                tbIcu.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                _context.Update(tbIcu);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(icuViewModel);
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var icu = await _context.TbIcus.FindAsync(id);

                // Delete associated TbBedMaster entries
                //var bedEntries = _context.TbBedMasters.Where(bed => bed.IcuId == icu.IcuId);
                //_context.TbBedMasters.RemoveRange(bedEntries);

                var bedEntries = _context.TbBedMasters.Where(bed => bed.IcuId == icu.IcuId);
                foreach (var bedEntry in bedEntries)
                {
                    bedEntry.LastUpdatedOn = DateTime.Now;
                    bedEntry.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    bedEntry.Status = false;

                    _context.Update(bedEntry);
                }

                icu.LastUpdatedOn = DateTime.Now;
                icu.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                icu.Status = false;

                _context.Update(icu);

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
