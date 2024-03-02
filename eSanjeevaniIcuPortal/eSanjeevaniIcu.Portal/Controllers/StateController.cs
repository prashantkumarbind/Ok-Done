using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Portal.Controllers
{
    public class StateController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public StateController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            StateViewModel stateViewModel = new StateViewModel();
            try
            {
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                return View(stateViewModel);
            }
            return View(stateViewModel);
        }

        
        [HttpGet]
        public ActionResult GetStateList()
        {
            try
            {
                var tbStateMaster = _context.TbStates
                    .Where(state => state.Status == true)
                    .Select(state => new StateViewModel
                    {
                        StateId = state.StateId,
                        StateName = state.StateName,
                        HasStateAdmin = _context.TbStateAdmins.Any(sa => sa.StateId == state.StateId && sa.Status == true)
                    }).ToList();

                return Json(new { data = tbStateMaster });
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                return Json(new { data = new List<StateViewModel>() });
            }
        }


        public IActionResult Create()
        {
            StateViewModel model = new StateViewModel();
            model.lstStateMasterMain = _context.TbIndianStates.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName }).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(StateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingState = _context.TbStates
                        .FirstOrDefault(x => x.StateName == model.StateName);

                    if (existingState != null)
                    {
                        // If the state exists and is soft-deleted, update its status to true
                        if (!existingState.Status)
                        {
                            existingState.Status = true;
                            existingState.LastUpdatedOn = DateTime.Now;
                            existingState.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;

                            _context.Update(existingState);
                            _context.SaveChanges();

                            TempData["SubmitResult"] = Common.MsgSaveSuccess;
                            return RedirectToAction(nameof(Index));
                        }

                        // If the state already exists and is active, show an error
                        ModelState.AddModelError("StateName", "State already exists.");
                        TempData["StateExist"] = "State already exists.";
                        model.lstStateMasterMain = _context.TbIndianStates
                            .Where(x => x.Status == true)
                            .Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName })
                            .ToList();
                        return View(model);
                    }

                    TbState newState = new TbState();

                    newState.StateCode = model.StateName;
                    newState.StateName = model.StateName;
                    newState.CreatedOn = DateTime.Now;
                    newState.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    newState.Status = true;

                    _context.TbStates.Add(newState);
                    _context.SaveChanges();

                    TempData["SubmitResult"] = Common.MsgSaveSuccess;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the state.");
                model.lstStateMasterMain = _context.TbIndianStates
                    .Where(x => x.Status == true)
                    .Select(s => new SelectListItem { Value = s.StateId.ToString(), Text = s.StateName })
                    .ToList();
            }

            return View(model);
        }

        

        [HttpGet]
        public JsonResult CheckStateAdmin(string stateName)
        {
            bool hasStateAdmin = _context.TbStates
                .Join(_context.TbStateAdmins,
                      state => state.StateId,
                      stateAdmin => stateAdmin.StateId,
                      (state, stateAdmin) => new { State = state, StateAdmin = stateAdmin })
                .Any(joined => joined.StateAdmin.Status == true && joined.State.StateName == stateName);

            return Json(new { hasStateAdmin });
        }




        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var state = await _context.TbStates.FindAsync(id);

                if (state == null)
                {
                    return NotFound();
                }

                var stateAdmins = await _context.TbStateAdmins
                .Where(sa => sa.StateId == id)
                .ToListAsync();

                foreach (var stateAdmin in stateAdmins)
                {
                    stateAdmin.Status = false;
                    _context.Update(stateAdmin);
                }

                state.LastUpdatedOn = DateTime.Now;
                state.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                state.Status = false;
                _context.Update(state);
                //_context.Remove(state);

                await _context.SaveChangesAsync();
                return Json(new { Status = true });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false });
            }

        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    StateViewModel stateViewModel = new StateViewModel();
        //    var tbState = await _context.TbStates.FirstOrDefaultAsync(m => m.StateId == id);

        //    if (tbState == null)
        //    {
        //        return NotFound();
        //    }

        //    stateViewModel.StateId = tbState.StateId;
        //    stateViewModel.StateName = tbState.StateName;

        //    return View(stateViewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete(int id)
        //{
        //    try
        //    {
        //        TbState tbState = _context.TbStates.Find(id);

        //        if (tbState != null)
        //        {
        //            _context.TbStates.Remove(tbState);
        //            int res = _context.SaveChanges();

        //            if (res > 0)
        //            {
        //                TempData["SubmitResult"] = Common.MsgDeleteSuccess;
        //                return RedirectToAction("Index");
        //            }
        //        }
        //        else
        //        {
        //            TempData["SubmitResult"] = "Record not found!";
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["SubmitResult"] = "An error occurred while deleting the record.";
        //        return RedirectToAction("Index");
        //    }

        //    TempData["SubmitResult"] = "An error occurred while deleting the record.";
        //    return RedirectToAction("Index");
        //}





    }
}
