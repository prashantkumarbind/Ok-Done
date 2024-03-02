using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SecurityGeneratePwd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace APDelaneyII.Portal.Controllers
{
    [SessionTimeoutAttribute]
    public class MenuMastersController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public MenuMastersController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }

        // GET: MenuMasters
        public ActionResult Index()
        {
            int principalId = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
            MenuMasterViewModel menuMasterViewModel = new MenuMasterViewModel();
            try
            {
                var param1 = new SqlParameter("@PrincipalId", principalId);
                param1.SqlDbType = System.Data.SqlDbType.Int;
                var menuMasterEntityList = _context.MenuMasterEntity.FromSqlRaw("Exec [usp_GetMenus] @PrincipalId", param1).ToList();

                menuMasterViewModel.lstMenuMaster = (from lstMenuMaster in menuMasterEntityList
                                                     select new MenuMasterViewModel
                                                     {
                                                         MenuId = lstMenuMaster.MenuId,
                                                         Name = lstMenuMaster.Name,
                                                         Description = lstMenuMaster.Description,
                                                         Sequence = lstMenuMaster.Sequence,
                                                         Title = lstMenuMaster.Title,
                                                         Icon = lstMenuMaster.Icon,
                                                         Url = lstMenuMaster.Url,
                                                         ParentMenuId = lstMenuMaster.ParentMenuId,
                                                         ParentMenuName = lstMenuMaster.ParentMenuName,
                                                         HasSubMenu = lstMenuMaster.HasSubMenu,
                                                         Status = lstMenuMaster.Status
                                                     }).ToList();

            }
            catch (SqlException ex)
            {
                string message = ex.Message.ToString();
                return null;
            }
            catch (Exception ex1)
            {
                string message = ex1.Message.ToString();
                return null;
            }
            return View(menuMasterViewModel);
        }

        // GET: MenuMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbMenuMaster = await _context.TbMenuMasters
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (tbMenuMaster == null)
            {
                return NotFound();
            }
            MenuMasterViewModel menuMasterViewModel = new MenuMasterViewModel();
            menuMasterViewModel.MenuId = tbMenuMaster.MenuId;
            menuMasterViewModel.Name = tbMenuMaster.Name;
            menuMasterViewModel.Description = tbMenuMaster.Description;
            menuMasterViewModel.Sequence = tbMenuMaster.Sequence;
            menuMasterViewModel.Title = tbMenuMaster.Title;
            menuMasterViewModel.Icon = tbMenuMaster.Icon;
            menuMasterViewModel.Url = tbMenuMaster.Url;
            menuMasterViewModel.ParentMenuId = tbMenuMaster.ParentMenuId;
            if (tbMenuMaster.ParentMenuId != null)
            {
                var tbMenuMaster2 = await _context.TbMenuMasters
                .FirstOrDefaultAsync(m => m.MenuId == tbMenuMaster.ParentMenuId);
                menuMasterViewModel.ParentMenuName = tbMenuMaster2.Description;
            }
            else
            {
                menuMasterViewModel.ParentMenuName = "N/A";
            }
            menuMasterViewModel.HasSubMenu = tbMenuMaster.HasSubMenu;
            menuMasterViewModel.Status = tbMenuMaster.Status;
            return View(menuMasterViewModel);
        }

        // GET: MenuMasters/Create
        public IActionResult Create()
        {
            MenuMasterViewModel menuMasterViewModel = new MenuMasterViewModel();
            menuMasterViewModel.lstParentMenus = (from menus in _context.TbMenuMasters
                                                  where menus.Status == true
                                                  orderby menus.Sequence ascending
                                                  select new SelectListItem()
                                                  {
                                                      Value = Convert.ToString(menus.MenuId),
                                                      Text = Convert.ToString(menus.Sequence) + ' ' + '-' + ' ' + menus.Description
                                                  }).ToList();
            return View(menuMasterViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(MenuMasterViewModel mmVM)
        {
            try
            {
                TbMenuMaster tbMenuMaster = new TbMenuMaster();
                tbMenuMaster.Name = mmVM.Name;
                tbMenuMaster.Description = mmVM.Description;
                tbMenuMaster.Sequence = mmVM.Sequence;
                tbMenuMaster.Title = mmVM.Title;
                tbMenuMaster.Icon = mmVM.Icon;
                tbMenuMaster.Url = mmVM.Url;
                tbMenuMaster.ParentMenuId = mmVM.ParentMenuId;
                tbMenuMaster.HasSubMenu = false;
                tbMenuMaster.Status = true;
                if (ModelState.IsValid)
                {
                    _context.Add(tbMenuMaster);
                    await _context.SaveChangesAsync();
                }
                TempData["SubmitResult"] = Common.MsgSaveSuccess;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                mmVM.lstParentMenus = (from menus in _context.TbMenuMasters
                                       where menus.Status == true
                                       orderby menus.Sequence ascending
                                       select new SelectListItem()
                                       {
                                           Value = Convert.ToString(menus.MenuId),
                                           Text = Convert.ToString(menus.Sequence) + ' ' + '-' + ' ' + menus.Description
                                       }).ToList();
                return View(mmVM);
            }
        }

        // GET: MenuMasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbMenuMaster = await _context.TbMenuMasters.FindAsync(id);
            if (tbMenuMaster == null)
            {
                return NotFound();
            }
            else
            {
                MenuMasterViewModel menuMasterViewModel = new MenuMasterViewModel();
                menuMasterViewModel.MenuId = tbMenuMaster.MenuId;
                menuMasterViewModel.Name = tbMenuMaster.Name;
                menuMasterViewModel.Description = tbMenuMaster.Description;
                menuMasterViewModel.Sequence = tbMenuMaster.Sequence;
                menuMasterViewModel.Title = tbMenuMaster.Title;
                menuMasterViewModel.Icon = tbMenuMaster.Icon;
                menuMasterViewModel.Url = tbMenuMaster.Url;
                menuMasterViewModel.ParentMenuId = tbMenuMaster.ParentMenuId;
                menuMasterViewModel.HasSubMenu = tbMenuMaster.HasSubMenu;
                menuMasterViewModel.Status = tbMenuMaster.Status;
                menuMasterViewModel.lstParentMenus = (from menus in _context.TbMenuMasters
                                                      where menus.Status == true
                                                      orderby menus.Sequence ascending
                                                      select new SelectListItem()
                                                      {
                                                          Value = Convert.ToString(menus.MenuId),
                                                          Text = Convert.ToString(menus.Sequence) + ' ' + '-' + ' ' + menus.Description
                                                      }).ToList();
                return View(menuMasterViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuMasterViewModel mmVM)
        {
            if (id != mmVM.MenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TbMenuMaster tbMenuMaster = new TbMenuMaster();
                    tbMenuMaster.MenuId = mmVM.MenuId;
                    tbMenuMaster.Name = mmVM.Name;
                    tbMenuMaster.Description = mmVM.Description;
                    tbMenuMaster.Sequence = mmVM.Sequence;
                    tbMenuMaster.Title = mmVM.Title;
                    tbMenuMaster.Icon = mmVM.Icon;
                    tbMenuMaster.Url = mmVM.Url;
                    tbMenuMaster.ParentMenuId = mmVM.ParentMenuId;
                    tbMenuMaster.HasSubMenu = mmVM.HasSubMenu;
                    tbMenuMaster.Status = true;
                    _context.Update(tbMenuMaster);
                    await _context.SaveChangesAsync();
                    TempData["SubmitResult"] = Common.MsgUpdateSuccess;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbMenuMasterExists(mmVM.MenuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mmVM);
        }

        // GET: MenuMasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MenuMasterViewModel menuMasterViewModel = new MenuMasterViewModel();
            var tbMenuMaster = await _context.TbMenuMasters
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (tbMenuMaster == null)
            {
                return NotFound();
            }
            menuMasterViewModel.MenuId = tbMenuMaster.MenuId;
            menuMasterViewModel.Name = tbMenuMaster.Name;
            menuMasterViewModel.Description = tbMenuMaster.Description;
            menuMasterViewModel.Sequence = tbMenuMaster.Sequence;
            menuMasterViewModel.Title = tbMenuMaster.Title;
            menuMasterViewModel.Icon = tbMenuMaster.Icon;
            menuMasterViewModel.Url = tbMenuMaster.Url;
            menuMasterViewModel.ParentMenuId = tbMenuMaster.ParentMenuId;
            if (tbMenuMaster.ParentMenuId != null)
            {
                var tbMenuMaster2 = await _context.TbMenuMasters
                .FirstOrDefaultAsync(m => m.MenuId == tbMenuMaster.ParentMenuId);
                menuMasterViewModel.ParentMenuName = tbMenuMaster2.Description;
            }
            else
            {
                menuMasterViewModel.ParentMenuName = "N/A";
            }
            menuMasterViewModel.HasSubMenu = tbMenuMaster.HasSubMenu;
            menuMasterViewModel.Status = tbMenuMaster.Status;
            return View(menuMasterViewModel);
        }

        //// POST: MenuMasters/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var tbMenuMaster = await _context.TbMenuMasters.FindAsync(id);
        //    tbMenuMaster.Status = false;
        //    //_context.TbMenuMaster.Remove(tbMenuMaster);
        //    _context.Update(tbMenuMaster);
        //    await _context.SaveChangesAsync();
        //    TempData["SubmitResult"] = Common.MsgDeleteSuccess;
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        public JsonResult DeleteMenuMasters(int id)
        {
            try
            {
                TbMenuMaster tbMenuMaster = _context.TbMenuMasters.Find(id);
                tbMenuMaster.Status = false;
                 _context.TbMenuMasters.Update(tbMenuMaster);
                int res = _context.SaveChanges();
                if (res > 0)
                {
                    TempData["SubmitResult"] = Common.MsgDeleteSuccess;

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message.ToString() });
            }
            return Json(new { success = true, responseText = "Record Deleted Successfully!" });
        }

        private bool TbMenuMasterExists(int id)
        {
            return _context.TbMenuMasters.Any(e => e.MenuId == id);
        }
    }
}
