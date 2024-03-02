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
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Controllers
{
    [SessionTimeoutAttribute]
    public class PatientMasterController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public PatientMasterController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }


        public ActionResult Index()
        {
            PatientMasterViewModel patientMasterViewModel = new PatientMasterViewModel();
            try
            {
                patientMasterViewModel.lstPatientMasterViewModel = (from patientMaster in _context.TbPatientMasters
                                                                    where patientMaster.Status == true
                                                                    select new PatientMasterViewModel
                                                                    {
                                                                        PatientId = patientMaster.PatientId,
                                                                        PatientName = patientMaster.PatientName,
                                                                        PatientCode = patientMaster.PatientCode,
                                                                        Status = patientMaster.Status,
                                                                        StatusName = patientMaster.Status == true ? "Active" : "Inactive",
                                                                        BedNo = patientMaster.BedNo,
                                                                        AdmissionDate = patientMaster.AdmissionDate,
                                                                        City = patientMaster.City,
                                                                    }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientMasterViewModel.lstPatientMasterViewModel = new List<PatientMasterViewModel>();
                return View(patientMasterViewModel);
            }
            return View(patientMasterViewModel);
        }

        // GET: MenuMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            PatientMasterViewModel patientMasterViewModel = new PatientMasterViewModel();
            try
            {
                patientMasterViewModel = (from patientMaster in _context.TbPatientMasters
                                          join spokeHospital in _context.TbSpokeHospitals on patientMaster.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                          join userMaster in _context.TbUserMasters on patientMaster.CreatedBy equals userMaster.UserId
                                          where patientMaster.Status == true && patientMaster.PatientId == id
                                          select new PatientMasterViewModel
                                          {
                                              PatientId = patientMaster.PatientId,
                                              PatientName = patientMaster.PatientName,
                                              PatientCode = patientMaster.PatientCode,
                                              SpokeHospitalId = patientMaster.SpokeHospitalId,
                                              SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                              Description = patientMaster.Description,
                                              ContactNo = patientMaster.ContactNo,
                                              Address = patientMaster.Address,
                                              City = patientMaster.City,
                                              State = patientMaster.State,
                                              Status = patientMaster.Status,
                                              StatusName = patientMaster.Status == true ? "Active" : "Inactive",
                                              BedNo = patientMaster.BedNo,
                                              Pin = patientMaster.Pin,
                                              Age = patientMaster.Age,
                                              Gender = patientMaster.Gender,
                                              AdmissionDate = patientMaster.AdmissionDate,
                                              CreatedOn = patientMaster.CreatedOn,
                                              CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientMasterViewModel = new PatientMasterViewModel();
                return View(patientMasterViewModel);
            }
            return View(patientMasterViewModel);
        }

        // GET: MenuMasters/Create
        public IActionResult Create()
        {
            PatientMasterViewModel patientMasterViewModel = new PatientMasterViewModel();
            try
            {
                patientMasterViewModel.AdmissionDate = DateTime.Now.Date;
                patientMasterViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientMasterViewModel = new PatientMasterViewModel();
                patientMasterViewModel.lstSpokeHospital = new List<SelectListItem>();
                return View(patientMasterViewModel);
            }
            return View(patientMasterViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PatientMasterViewModel patientMasterViewModel)
        {
            try
            {
                TbPatientMaster patientMaster = new TbPatientMaster();
                patientMaster.SpokeHospitalId = patientMasterViewModel.SpokeHospitalId;
                patientMaster.PatientName = patientMasterViewModel.PatientName;
                patientMaster.PatientCode = GeneratePatientCode();
                patientMaster.Description = patientMasterViewModel.Description;
                patientMaster.ContactNo = patientMasterViewModel.ContactNo;
                patientMaster.Address = patientMasterViewModel.Address;
                patientMaster.City = patientMasterViewModel.City;
                patientMaster.State = patientMasterViewModel.State;
                patientMaster.Pin = patientMasterViewModel.Pin;
                patientMaster.Age = patientMasterViewModel.Age;
                patientMaster.Gender = patientMasterViewModel.Gender;
                patientMaster.BedNo = patientMasterViewModel.BedNo;
                patientMaster.AdmissionDate = patientMasterViewModel.AdmissionDate;
                patientMaster.CreatedOn = DateTime.Now;
                patientMaster.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                patientMaster.Status = true;
                _context.Add(patientMaster);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                patientMasterViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                return View(patientMasterViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: MenuMasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            PatientMasterViewModel patientMasterViewModel = new PatientMasterViewModel();
            try
            {
                patientMasterViewModel = (from patientMaster in _context.TbPatientMasters
                                          where patientMaster.Status == true && patientMaster.PatientId == id
                                          select new PatientMasterViewModel
                                          {
                                              PatientId = patientMaster.PatientId,
                                              PatientName = patientMaster.PatientName,
                                              PatientCode = patientMaster.PatientCode,
                                              SpokeHospitalId = patientMaster.SpokeHospitalId,
                                              Description = patientMaster.Description,
                                              ContactNo = patientMaster.ContactNo,
                                              Address = patientMaster.Address,
                                              City = patientMaster.City,
                                              Age = patientMaster.Age,
                                              Gender = patientMaster.Gender,
                                              State = patientMaster.State,
                                              BedNo = patientMaster.BedNo,
                                              Pin = patientMaster.Pin,
                                              AdmissionDate = patientMaster.AdmissionDate,
                                          }).FirstOrDefault();
                patientMasterViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientMasterViewModel = new PatientMasterViewModel();
                patientMasterViewModel.lstSpokeHospital = new List<SelectListItem>();
                return View(patientMasterViewModel);
            }
            return View(patientMasterViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientMasterViewModel patientMasterViewModel)
        {
            try
            {
                TbPatientMaster patientMaster = _context.TbPatientMasters.Find(id);
                patientMaster.SpokeHospitalId = patientMasterViewModel.SpokeHospitalId;
                patientMaster.PatientName = patientMasterViewModel.PatientName;
                patientMaster.Description = patientMasterViewModel.Description;
                patientMaster.ContactNo = patientMasterViewModel.ContactNo;
                patientMaster.Address = patientMasterViewModel.Address;
                patientMaster.City = patientMasterViewModel.City;
                patientMaster.State = patientMasterViewModel.State;
                patientMaster.Pin = patientMasterViewModel.Pin;
                patientMaster.Age = patientMasterViewModel.Age;
                patientMaster.Gender = patientMasterViewModel.Gender;
                patientMaster.BedNo = patientMasterViewModel.BedNo;
                patientMaster.AdmissionDate = patientMasterViewModel.AdmissionDate;
                patientMaster.LastUpdatedOn = DateTime.Now;
                patientMaster.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                patientMaster.Status = true;
                _context.Update(patientMaster);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                patientMasterViewModel.lstSpokeHospital = _context.TbSpokeHospitals.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.SpokeHospitalId.ToString(), Text = s.SpokeHospitalName }).ToList();
                return View(patientMasterViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: MenuMasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            PatientMasterViewModel patientMasterViewModel = new PatientMasterViewModel();
            try
            {
                patientMasterViewModel = (from patientMaster in _context.TbPatientMasters
                                          join spokeHospital in _context.TbSpokeHospitals on patientMaster.SpokeHospitalId equals spokeHospital.SpokeHospitalId
                                          join userMaster in _context.TbUserMasters on patientMaster.CreatedBy equals userMaster.UserId
                                          where patientMaster.Status == true && patientMaster.PatientId == id
                                          select new PatientMasterViewModel
                                          {
                                              PatientId = patientMaster.PatientId,
                                              PatientName = patientMaster.PatientName,
                                              PatientCode = patientMaster.PatientCode,
                                              SpokeHospitalId = patientMaster.SpokeHospitalId,
                                              SpokeHospitalName = spokeHospital.SpokeHospitalName,
                                              Description = patientMaster.Description,
                                              ContactNo = patientMaster.ContactNo,
                                              Address = patientMaster.Address,
                                              City = patientMaster.City,
                                              State = patientMaster.State,
                                              Status = patientMaster.Status,
                                              StatusName = patientMaster.Status == true ? "Active" : "Inactive",
                                              BedNo = patientMaster.BedNo,
                                              Pin = patientMaster.Pin,
                                              Age = patientMaster.Age,
                                              Gender = patientMaster.Gender,
                                              AdmissionDate = patientMaster.AdmissionDate,
                                              CreatedOn = patientMaster.CreatedOn,
                                              CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientMasterViewModel = new PatientMasterViewModel();
                return View(patientMasterViewModel);
            }
            return View(patientMasterViewModel);
        }

        // POST: MenuMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientMaster = await _context.TbPatientMasters.FindAsync(id);
            patientMaster.LastUpdatedOn = DateTime.Now;
            patientMaster.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
            patientMaster.Status = false;
            _context.Update(patientMaster);
            await _context.SaveChangesAsync();
            TempData["SubmitResult"] = Common.MsgDeleteSuccess;
            return RedirectToAction(nameof(Index));
        }
        public string GeneratePatientCode()
        {
            int patientNo = 1001;
            int patientCount = _context.TbPatientMasters.Count();
            if (patientCount > 0)
            {
                patientNo = patientNo + patientCount;
            }
            string PatientCode = "P" + patientNo.ToString();
            return PatientCode;
        }
    }
}
