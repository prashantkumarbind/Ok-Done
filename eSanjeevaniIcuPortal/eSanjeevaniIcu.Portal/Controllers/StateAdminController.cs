using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.EntityFrameworkCore;

namespace eSanjeevaniIcu.Portal.Controllers
{
    public class StateAdminController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public StateAdminController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: StateAdmin/Create
        public IActionResult Create(int id)
        {
            StateAdminViewModel stateAdminViewModel = new StateAdminViewModel();
            try
            {
                stateAdminViewModel.StateId = id;
                //stateAdminViewModel.lstStateAdmin = _context.TbStateAdmins.Where(x => x.Status == true).Select(s => new SelectListItem { Value = s.StateAdminId.ToString(), Text = s.FirstName }).ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                stateAdminViewModel.lstStateAdmin = new List<SelectListItem>();
                return View(stateAdminViewModel);
            }
            return View(stateAdminViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StateAdminViewModel stateAdminViewModel)
        {
            try
            {
                TbStateAdmin tbStateAdmin = new TbStateAdmin();
                tbStateAdmin.StateId = stateAdminViewModel.StateId;
                tbStateAdmin.SurName = stateAdminViewModel.SurName;
                tbStateAdmin.FirstName = stateAdminViewModel.FirstName;
                tbStateAdmin.MiddleName = stateAdminViewModel.MiddleName;
                tbStateAdmin.LastName = stateAdminViewModel.LastName;
                tbStateAdmin.PhoneNumber = stateAdminViewModel.PhoneNumber;
                tbStateAdmin.Email = stateAdminViewModel.Email;
                tbStateAdmin.Designation = stateAdminViewModel.Designation;
                tbStateAdmin.PlaceOfWork = stateAdminViewModel.PlaceOfWork;
                tbStateAdmin.Address = stateAdminViewModel.Address;
                tbStateAdmin.CreatedOn = DateTime.Now;
                tbStateAdmin.CreatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                tbStateAdmin.Status = true;

                _context.Add(tbStateAdmin);
                await _context.SaveChangesAsync();
                TempData["SubmitResult"] = Common.MsgUpdateSuccess;
            }
            catch (Exception ex)
            {
                return View(stateAdminViewModel);
            }
            // return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "State");
        }

        // GET: StateAdmin/Details/5
        public IActionResult Details(int? id)
        {
            StateAdminViewModel stateAdminViewModel = new StateAdminViewModel();
            try
            {
                stateAdminViewModel = (from stateAdmins in _context.TbStateAdmins
                                       join state in _context.TbStates on stateAdmins.StateId equals state.StateId
                                       where stateAdmins.Status == true && stateAdmins.StateId == id
                                       select new StateAdminViewModel
                                       {
                                           StateAdminId = stateAdmins.StateAdminId,
                                           StateId = stateAdmins.StateId,
                                           StateName = state.StateName,
                                           SurName = stateAdmins.SurName,
                                           FirstName = stateAdmins.FirstName,
                                           MiddleName = stateAdmins.MiddleName,
                                           LastName = stateAdmins.LastName,
                                           PhoneNumber = stateAdmins.PhoneNumber,
                                           Email = stateAdmins.Email,
                                           Designation = stateAdmins.Designation,
                                           PlaceOfWork = stateAdmins.PlaceOfWork,
                                           Address = stateAdmins.Address,

                                       }).FirstOrDefault();
                
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                
                stateAdminViewModel = new StateAdminViewModel();
                return View(stateAdminViewModel);
            }
            return View(stateAdminViewModel);
        }


        // GET: StateAdmin/Edit/5
        public Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return Task.FromResult<IActionResult>(RedirectToAction("Error"));
            }

            StateAdminViewModel stateAdminViewModel;
            try
            {
                stateAdminViewModel = _context.TbStateAdmins
                    .Where(sa => sa.Status == true && sa.StateAdminId == id)
                    .Select(sa => new StateAdminViewModel
                    {
                        StateAdminId = sa.StateAdminId,
                        StateId = sa.StateId,
                        SurName = sa.SurName,
                        FirstName = sa.FirstName,
                        MiddleName = sa.MiddleName,
                        LastName = sa.LastName,
                        PhoneNumber = sa.PhoneNumber,
                        Email = sa.Email,
                        Designation = sa.Designation,
                        PlaceOfWork = sa.PlaceOfWork,
                        Address = sa.Address,
                    })
                    .FirstOrDefault();

                if (stateAdminViewModel == null)
                {
                    return Task.FromResult<IActionResult>(RedirectToAction("NotFound"));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                return Task.FromResult<IActionResult>(RedirectToAction("Error"));
            }

            return Task.FromResult<IActionResult>(View(stateAdminViewModel));
        }

        // POST: StateAdmin/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, StateAdminViewModel stateAdminViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TbStateAdmin tbStateAdmin = await _context.TbStateAdmins.FindAsync(id);

                    tbStateAdmin.SurName = stateAdminViewModel.SurName;
                    tbStateAdmin.FirstName = stateAdminViewModel.FirstName;
                    tbStateAdmin.MiddleName = stateAdminViewModel.MiddleName;
                    tbStateAdmin.LastName = stateAdminViewModel.LastName;
                    tbStateAdmin.PhoneNumber = stateAdminViewModel.PhoneNumber;
                    tbStateAdmin.Email = stateAdminViewModel.Email;
                    tbStateAdmin.Designation = stateAdminViewModel.Designation;
                    tbStateAdmin.PlaceOfWork = stateAdminViewModel.PlaceOfWork;
                    tbStateAdmin.Address = stateAdminViewModel.Address;
                    tbStateAdmin.LastUpdatedOn = DateTime.Now;
                    tbStateAdmin.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;

                    _context.Update(tbStateAdmin);
                    await _context.SaveChangesAsync();

                    TempData["SubmitResult"] = Common.MsgUpdateSuccess;
                    return RedirectToAction("Index", "State");
                }

                return View(stateAdminViewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteStateAdmin(int id)
        {
            try
            {
                var stateAdmin = await _context.TbStateAdmins.FindAsync(id);

                if (stateAdmin == null)
                {
                    return NotFound();
                }
                stateAdmin.LastUpdatedOn = DateTime.Now;
                stateAdmin.LastUpdatedBy = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                stateAdmin.Status = false;
                _context.Update(stateAdmin);
                //_context.TbStateAdmins.Remove(stateAdmin);

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
