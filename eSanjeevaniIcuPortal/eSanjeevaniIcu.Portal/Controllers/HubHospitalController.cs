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
    public class HubHospitalController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public HubHospitalController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }


        public ActionResult Index()
        {
            HubHospitalViewModel hubHospitalViewModel = new HubHospitalViewModel();
            try
            {
                hubHospitalViewModel.lstHubHospitalViewModel = (from hubHospital in _context.TbHubHospitals
                                                                where hubHospital.Status == true
                                                                select new HubHospitalViewModel
                                                                {
                                                                    HubHospitalId = hubHospital.HubHospitalId,
                                                                    HubHospitalName = hubHospital.HubHospitalName,
                                                                    ContactNo = hubHospital.ContactNo,
                                                                    City = hubHospital.City,
                                                                    State = hubHospital.State,
                                                                    Address = hubHospital.Address,
                                                                    Status = hubHospital.Status,
                                                                    StatusName = hubHospital.Status == true ? "Active" : "Inactive",
                                                                }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                hubHospitalViewModel.lstHubHospitalViewModel = new List<HubHospitalViewModel>();
                return View(hubHospitalViewModel);
            }
            return View(hubHospitalViewModel);
        }

        // GET: MenuMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            HubHospitalViewModel hubHospitalViewModel = new HubHospitalViewModel();
            try
            {
                hubHospitalViewModel = (from hubHospital in _context.TbHubHospitals
                                        join userMaster in _context.TbUserMasters on hubHospital.CreatedBy equals userMaster.UserId
                                        where hubHospital.Status == true && hubHospital.HubHospitalId == id
                                        select new HubHospitalViewModel
                                        {
                                            HubHospitalId = hubHospital.HubHospitalId,
                                            HubHospitalName = hubHospital.HubHospitalName,
                                            ContactNo = hubHospital.ContactNo,
                                            Address = hubHospital.Address,
                                            City = hubHospital.City,
                                            State = hubHospital.State,
                                            Pin = hubHospital.Pin,
                                            Status = hubHospital.Status,
                                            StatusName = hubHospital.Status == true ? "Active" : "Inactive",
                                            CreatedOn = hubHospital.CreatedOn,
                                            CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                        }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                hubHospitalViewModel = new HubHospitalViewModel();
                return View(hubHospitalViewModel);
            }
            return View(hubHospitalViewModel);
        }

        // GET: MenuMasters/Create
        public IActionResult Create()
        {
            HubHospitalViewModel hubHospitalViewModel = new HubHospitalViewModel();
            try
            {
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                hubHospitalViewModel = new HubHospitalViewModel();
                return View(hubHospitalViewModel);
            }
            return View(hubHospitalViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(HubHospitalViewModel hubHospitalViewModel)
        {
            try
            {
                TbHubHospital hubHospital = new TbHubHospital();
                hubHospital.HubHospitalName = hubHospitalViewModel.HubHospitalName;
                hubHospital.ContactNo = hubHospitalViewModel.ContactNo;
                hubHospital.Address = hubHospitalViewModel.Address;
                hubHospital.City = hubHospitalViewModel.City;
                hubHospital.State = hubHospitalViewModel.State;
                hubHospital.Pin = hubHospitalViewModel.Pin;
                hubHospital.CreatedOn = DateTime.Now;
                hubHospital.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                hubHospital.Status = true;
                _context.Add(hubHospital);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(hubHospitalViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: MenuMasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            HubHospitalViewModel hubHospitalViewModel = new HubHospitalViewModel();
            try
            {
                hubHospitalViewModel = (from hubHospital in _context.TbHubHospitals
                                        where hubHospital.Status == true && hubHospital.HubHospitalId == id
                                        select new HubHospitalViewModel
                                        {
                                            HubHospitalId = hubHospital.HubHospitalId,
                                            HubHospitalName = hubHospital.HubHospitalName,
                                            ContactNo = hubHospital.ContactNo,
                                            Address = hubHospital.Address,
                                            City = hubHospital.City,
                                            State = hubHospital.State,
                                            Pin = hubHospital.Pin,
                                        }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                hubHospitalViewModel = new HubHospitalViewModel();
                return View(hubHospitalViewModel);
            }
            return View(hubHospitalViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HubHospitalViewModel hubHospitalViewModel)
        {
            try
            {
                TbHubHospital hubHospital = _context.TbHubHospitals.Find(id);
                hubHospital.HubHospitalName = hubHospitalViewModel.HubHospitalName;
                hubHospital.ContactNo = hubHospitalViewModel.ContactNo;
                hubHospital.Address = hubHospitalViewModel.Address;
                hubHospital.City = hubHospitalViewModel.City;
                hubHospital.State = hubHospitalViewModel.State;
                hubHospital.Pin = hubHospitalViewModel.Pin;
                hubHospital.LastUpdatedOn = DateTime.Now;
                hubHospital.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                hubHospital.Status = true;
                _context.Update(hubHospital);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(hubHospitalViewModel);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: MenuMasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            HubHospitalViewModel hubHospitalViewModel = new HubHospitalViewModel();
            try
            {
                hubHospitalViewModel = (from hubHospital in _context.TbHubHospitals
                                        join userMaster in _context.TbUserMasters on hubHospital.CreatedBy equals userMaster.UserId
                                        where hubHospital.Status == true && hubHospital.HubHospitalId == id
                                        select new HubHospitalViewModel
                                        {
                                            HubHospitalId = hubHospital.HubHospitalId,
                                            HubHospitalName = hubHospital.HubHospitalName,
                                            ContactNo = hubHospital.ContactNo,
                                            Address = hubHospital.Address,
                                            City = hubHospital.City,
                                            State = hubHospital.State,
                                            Status = hubHospital.Status,
                                            StatusName = hubHospital.Status == true ? "Active" : "Inactive",
                                            Pin = hubHospital.Pin,
                                            CreatedOn = hubHospital.CreatedOn,
                                            CreatedByName = userMaster.LastName != null ? (userMaster.FirstName + " " + userMaster.LastName) : userMaster.FirstName,
                                        }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                hubHospitalViewModel = new HubHospitalViewModel();
                return View(hubHospitalViewModel);
            }
            return View(hubHospitalViewModel);
        }

        // POST: MenuMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hubHospital = await _context.TbHubHospitals.FindAsync(id);
            hubHospital.LastUpdatedOn = DateTime.Now;
            hubHospital.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
            hubHospital.Status = false;
            _context.Update(hubHospital);
            await _context.SaveChangesAsync();
            TempData["SubmitResult"] = Common.MsgDeleteSuccess;
            return RedirectToAction(nameof(Index));
        }

    }
}
