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
    public class PatientDetailsController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public PatientDetailsController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }

        // GET: MenuMasters
        public ActionResult Index()
        {
            PatientDetailsViewModel patientDetailsViewModel = new PatientDetailsViewModel();
            try
            {
                //var lstPatientMaster = _context.TbPatientMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.PatientId.ToString(), Text = s.PatientName }).ToList();
                patientDetailsViewModel.lstPatientDetailsViewModel = (from patientDetails in _context.TbPatientDetails
                                                                      join patientMaster in _context.TbPatientMasters on patientDetails.PatientId equals patientMaster.PatientId
                                                                      where patientDetails.Status == true
                                                                      select new PatientDetailsViewModel
                                                                      {
                                                                          PatientDetailId = patientDetails.PatientDetailId,
                                                                          PatientId = patientDetails.PatientId,
                                                                          PatientName = patientMaster.PatientName,
                                                                          Date = patientDetails.Date,
                                                                          StatusName = patientDetails.Status == true ? "Active" : "Inactive",
                                                                      }).ToList();
                patientDetailsViewModel.lstPatientDetailsViewModel = patientDetailsViewModel.lstPatientDetailsViewModel.GroupBy(g => new { g.PatientId, g.Date }).Select(s => s.Last()).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientDetailsViewModel.lstPatientDetailsViewModel = new List<PatientDetailsViewModel>();
                return View(patientDetailsViewModel);
            }
            return View(patientDetailsViewModel);
        }


        public async Task<IActionResult> Details(int? id, DateTime dt)
        {
            PatientDetailsViewModel patientDetailsViewModel = new PatientDetailsViewModel();
            try
            {
                patientDetailsViewModel.lstPatientStatus = _context.TbPatientStatuses.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.PatientStatusId.ToString(), Text = s.PatientStatusName }).ToList();

                patientDetailsViewModel.lstPatientDetailsViewModel = (from patientDetails in _context.TbPatientDetails
                                                                      join patientMaster in _context.TbPatientMasters on patientDetails.PatientId equals patientMaster.PatientId
                                                                      join userMaster in _context.TbUserMasters on patientMaster.CreatedBy equals userMaster.UserId
                                                                      where patientDetails.PatientId == id && patientDetails.Date == dt
                                                                      select new PatientDetailsViewModel
                                                                      {
                                                                          PatientDetailId = patientDetails.PatientDetailId,
                                                                          PatientId = patientDetails.PatientId,
                                                                          PatientName = patientMaster.PatientName,
                                                                          Date = patientDetails.Date,
                                                                          Temperature = patientDetails.Temperature,
                                                                          RrRespiratoryRate = patientDetails.RrRespiratoryRate,
                                                                          OxygenSaturationSpo2 = patientDetails.OxygenSaturationSpo2,
                                                                          BloodPressureDia = patientDetails.BloodPressureDia,
                                                                          BloodPressureSys = patientDetails.BloodPressureSys,
                                                                          HeartRate = patientDetails.HeartRate,
                                                                          PatientStatus = patientDetails.PatientStatus,
                                                                          PatientStatusName = "",//lstPatientStatus.Where(x => Convert.ToInt32(x.Value) == Convert.ToInt32(patientDetails.PatientStatus)).Select(s => s.Text).FirstOrDefault(),
                                                                          CreatedOn = patientDetails.CreatedOn,
                                                                          CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                                                          StatusName = patientDetails.Status == true ? "Active" : "Inactive",
                                                                      }).ToList();
                patientDetailsViewModel.Date = patientDetailsViewModel.lstPatientDetailsViewModel.FirstOrDefault().Date;
                patientDetailsViewModel.PatientId = patientDetailsViewModel.lstPatientDetailsViewModel.FirstOrDefault().PatientId;
                patientDetailsViewModel.PatientName = patientDetailsViewModel.lstPatientDetailsViewModel.FirstOrDefault().PatientName;
                patientDetailsViewModel.CreatedByName = patientDetailsViewModel.lstPatientDetailsViewModel.FirstOrDefault().CreatedByName;

            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientDetailsViewModel.lstPatientDetailsViewModel = new List<PatientDetailsViewModel>();
                return View(patientDetailsViewModel);
            }
            return View(patientDetailsViewModel);
        }

        // GET: MenuMasters/Create
        public IActionResult Create()
        {
            PatientDetailsViewModel patientDetailsViewModel = new PatientDetailsViewModel();
            try
            {
                patientDetailsViewModel.Date = DateTime.Now.Date;
                patientDetailsViewModel.lstPatientMaster = _context.TbPatientMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.PatientId.ToString(), Text = s.PatientName }).ToList();
                patientDetailsViewModel.lstPatientStatus = _context.TbPatientStatuses.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.PatientStatusId.ToString(), Text = s.PatientStatusName }).ToList();
            }
            catch (Exception ex)
            {
                patientDetailsViewModel.lstPatientMaster = new List<SelectListItem>();
                patientDetailsViewModel.lstPatientStatus = new List<SelectListItem>();
                return View(patientDetailsViewModel);
            }
            return View(patientDetailsViewModel);
        }

        [HttpPost]
        public JsonResult Create(List<PatientDetailsViewModel> lstPatientDetailsViewModel)
        {
            using (var transation = _context.Database.BeginTransaction())
            {
                try
                {
                    List<TbPatientDetail> lstTbPatientDetails = new List<TbPatientDetail>();
                    foreach (var item in lstPatientDetailsViewModel)
                    {
                        TbPatientDetail tbPatientDetail = new TbPatientDetail();
                        tbPatientDetail.PatientId = item.PatientId;
                        tbPatientDetail.Date = item.Date;
                        tbPatientDetail.Temperature = item.Temperature;
                        tbPatientDetail.RrRespiratoryRate = item.RrRespiratoryRate;
                        tbPatientDetail.OxygenSaturationSpo2 = item.OxygenSaturationSpo2;
                        tbPatientDetail.BloodPressureDia = item.BloodPressureDia;
                        tbPatientDetail.BloodPressureSys = item.BloodPressureSys;
                        tbPatientDetail.HeartRate = item.HeartRate;
                        tbPatientDetail.PatientStatus = item.PatientStatus;
                        tbPatientDetail.CreatedOn = DateTime.Now;
                        tbPatientDetail.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                        tbPatientDetail.Status = true;
                        lstTbPatientDetails.Add(tbPatientDetail);
                    }
                    _context.TbPatientDetails.AddRange(lstTbPatientDetails);
                    _context.SaveChanges();
                    transation.Commit();
                }
                catch (Exception ex)
                {
                    transation.Rollback();
                    return Json(new { success = false, responseText = ex.Message.ToString() });
                }
            }
            return Json(new { success = true, responseText = "Record Save Successfully." });
        }


        public async Task<IActionResult> Edit(int? id, DateTime dt)
        {
            PatientDetailsViewModel patientDetailsViewModel = new PatientDetailsViewModel();
            try
            {
                patientDetailsViewModel.lstPatientDetailsViewModel = (from patientDetails in _context.TbPatientDetails
                                                                      where patientDetails.PatientId == id && patientDetails.Date.Date == dt
                                                                      select new PatientDetailsViewModel
                                                                      {
                                                                          PatientDetailId = patientDetails.PatientDetailId,
                                                                          PatientId = patientDetails.PatientId,
                                                                          Date = patientDetails.Date,
                                                                          Temperature = patientDetails.Temperature,
                                                                          RrRespiratoryRate = patientDetails.RrRespiratoryRate,
                                                                          OxygenSaturationSpo2 = patientDetails.OxygenSaturationSpo2,
                                                                          BloodPressureDia = patientDetails.BloodPressureDia,
                                                                          BloodPressureSys = patientDetails.BloodPressureSys,
                                                                          HeartRate = patientDetails.HeartRate,
                                                                          PatientStatus = patientDetails.PatientStatus,
                                                                      }).ToList();
                patientDetailsViewModel.Date = patientDetailsViewModel.lstPatientDetailsViewModel.FirstOrDefault().Date;
                patientDetailsViewModel.PatientId = patientDetailsViewModel.lstPatientDetailsViewModel.FirstOrDefault().PatientId;
                patientDetailsViewModel.lstPatientMaster = _context.TbPatientMasters.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.PatientId.ToString(), Text = s.PatientName }).ToList();
                patientDetailsViewModel.lstPatientStatus = _context.TbPatientStatuses.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.PatientStatusId.ToString(), Text = s.PatientStatusName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientDetailsViewModel.lstPatientDetailsViewModel = new List<PatientDetailsViewModel>();
                patientDetailsViewModel.lstPatientMaster = new List<SelectListItem>();
                patientDetailsViewModel.lstPatientStatus = new List<SelectListItem>();
                return View(patientDetailsViewModel);
            }
            return View(patientDetailsViewModel);
        }

        [HttpPost]
        public JsonResult Edit(List<PatientDetailsViewModel> lstPatientDetailsViewModel)
        {
            using (var transation = _context.Database.BeginTransaction())
            {
                try
                {
                    var patientDetailIdList = _context.TbPatientDetails.Where(x => x.PatientId == lstPatientDetailsViewModel.FirstOrDefault().PatientId && x.Date == lstPatientDetailsViewModel.FirstOrDefault().Date).Select(s => s.PatientDetailId).ToList();
                    List<TbPatientDetail> lstTbPatientDetails = new List<TbPatientDetail>();
                    foreach (var item in lstPatientDetailsViewModel)
                    {
                        if (item.PatientDetailId != 0)
                        {
                            patientDetailIdList.Remove(item.PatientDetailId);
                            TbPatientDetail tbPatientDetail = _context.TbPatientDetails.Find(item.PatientDetailId);
                            tbPatientDetail.PatientId = item.PatientId;
                            tbPatientDetail.Date = item.Date;
                            tbPatientDetail.Temperature = item.Temperature;
                            tbPatientDetail.RrRespiratoryRate = item.RrRespiratoryRate;
                            tbPatientDetail.OxygenSaturationSpo2 = item.OxygenSaturationSpo2;
                            tbPatientDetail.BloodPressureDia = item.BloodPressureDia;
                            tbPatientDetail.BloodPressureSys = item.BloodPressureSys;
                            tbPatientDetail.HeartRate = item.HeartRate;
                            tbPatientDetail.PatientStatus = item.PatientStatus;
                            tbPatientDetail.LastUpdatedOn = DateTime.Now;
                            tbPatientDetail.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                            _context.Update(tbPatientDetail);
                            _context.SaveChanges();
                        }
                        else
                        {
                            TbPatientDetail tbPatientDetail = new TbPatientDetail();
                            tbPatientDetail.PatientId = item.PatientId;
                            tbPatientDetail.Date = item.Date;
                            tbPatientDetail.Temperature = item.Temperature;
                            tbPatientDetail.RrRespiratoryRate = item.RrRespiratoryRate;
                            tbPatientDetail.OxygenSaturationSpo2 = item.OxygenSaturationSpo2;
                            tbPatientDetail.BloodPressureDia = item.BloodPressureDia;
                            tbPatientDetail.BloodPressureSys = item.BloodPressureSys;
                            tbPatientDetail.HeartRate = item.HeartRate;
                            tbPatientDetail.PatientStatus = item.PatientStatus;
                            tbPatientDetail.CreatedOn = DateTime.Now;
                            tbPatientDetail.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                            tbPatientDetail.Status = true;
                            _context.TbPatientDetails.Add(tbPatientDetail);
                            _context.SaveChanges();
                        }
                    }
                    if (patientDetailIdList.Count > 0)
                    {
                        _context.TbPatientDetails.RemoveRange(_context.TbPatientDetails.Where(x => patientDetailIdList.Contains(x.PatientDetailId)));
                        _context.SaveChanges();
                    }
                    transation.Commit();
                }
                catch (Exception ex)
                {
                    transation.Rollback();
                    return Json(new { success = false, responseText = ex.Message.ToString() });
                }
            }
            return Json(new { success = true, responseText = "Record Updated Successfully." });
        }



        public async Task<IActionResult> Delete(int? id, DateTime dt)
        {
            PatientDetailsViewModel patientDetailsViewModel = new PatientDetailsViewModel();
            try
            {
                patientDetailsViewModel.lstPatientStatus = _context.TbPatientStatuses.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.PatientStatusId.ToString(), Text = s.PatientStatusName }).ToList();

                patientDetailsViewModel.lstPatientDetailsViewModel = (from patientDetails in _context.TbPatientDetails
                                                                      join patientMaster in _context.TbPatientMasters on patientDetails.PatientId equals patientMaster.PatientId
                                                                      join userMaster in _context.TbUserMasters on patientMaster.CreatedBy equals userMaster.UserId
                                                                      where patientDetails.PatientId == id && patientDetails.Date == dt
                                                                      select new PatientDetailsViewModel
                                                                      {
                                                                          PatientDetailId = patientDetails.PatientDetailId,
                                                                          PatientId = patientDetails.PatientId,
                                                                          PatientName = patientMaster.PatientName,
                                                                          Date = patientDetails.Date,
                                                                          Temperature = patientDetails.Temperature,
                                                                          RrRespiratoryRate = patientDetails.RrRespiratoryRate,
                                                                          OxygenSaturationSpo2 = patientDetails.OxygenSaturationSpo2,
                                                                          BloodPressureDia = patientDetails.BloodPressureDia,
                                                                          BloodPressureSys = patientDetails.BloodPressureSys,
                                                                          HeartRate = patientDetails.HeartRate,
                                                                          PatientStatus = patientDetails.PatientStatus,
                                                                          PatientStatusName = "",// lstPatientStatus.Where(x => x.Value == patientDetails.PatientStatus.ToString()).Select(s => s.Text).FirstOrDefault(),
                                                                          CreatedOn = patientDetails.CreatedOn,
                                                                          CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                                                          StatusName = patientDetails.Status == true ? "Active" : "Inactive",
                                                                      }).ToList();
                patientDetailsViewModel.Date = patientDetailsViewModel.lstPatientDetailsViewModel.FirstOrDefault().Date;
                patientDetailsViewModel.PatientId = patientDetailsViewModel.lstPatientDetailsViewModel.FirstOrDefault().PatientId;
                patientDetailsViewModel.PatientName = patientDetailsViewModel.lstPatientDetailsViewModel.FirstOrDefault().PatientName;
                patientDetailsViewModel.CreatedByName = patientDetailsViewModel.lstPatientDetailsViewModel.FirstOrDefault().CreatedByName;
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                patientDetailsViewModel.lstPatientDetailsViewModel = new List<PatientDetailsViewModel>();
                return View(patientDetailsViewModel);
            }
            return View(patientDetailsViewModel);
        }

        [HttpPost]
        public JsonResult DeleteConfirmed(int? id, DateTime dt)
        {
            try
            {
                var patientDetailList = _context.TbPatientDetails.Where(x => x.PatientId == id && x.Date == dt).ToList();
                foreach (var item in patientDetailList)
                {
                    TbPatientDetail tbPatientDetail = _context.TbPatientDetails.Find(item.PatientDetailId);
                    tbPatientDetail.Status = false;
                    tbPatientDetail.LastUpdatedOn = DateTime.Now;
                    tbPatientDetail.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    _context.Update(tbPatientDetail);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    responseText = ex.Message.ToString()
                });
            }
            return Json(new { success = true, responseText = "Record Deleted Successfully." });
        }



    }
}
