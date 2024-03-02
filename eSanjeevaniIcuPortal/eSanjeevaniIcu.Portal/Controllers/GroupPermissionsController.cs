
using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Data.ExtendedEntities;
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
    public class GroupPermissionsController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public GroupPermissionsController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            int groupId = 0;
            GroupPermissionViewModel groupPermissionViewModel = new GroupPermissionViewModel();
            try
            {
                var param1 = new SqlParameter("@GroupId", groupId);
                param1.SqlDbType = System.Data.SqlDbType.Int;
                var groupPermissionEntityList = _context.GroupPermissionEntity.FromSqlRaw("Exec [usp_GetGroupPermission] @GroupId", param1).ToList();

                groupPermissionViewModel.lstGroupPermissions = (from lstGroupPermission in groupPermissionEntityList
                                                                select new GroupPermissionViewModel
                                                                {
                                                                    //GroupPermissionId = lstGroupPermission.GroupPermissionId,
                                                                    GroupId = lstGroupPermission.GroupId,
                                                                    GroupName = lstGroupPermission.GroupName,
                                                                    MenuIds = lstGroupPermission.MenuIds,
                                                                    MenuNames = lstGroupPermission.MenuNames,
                                                                    Status = lstGroupPermission.Status,
                                                                    StatusName = lstGroupPermission.StatusName
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
            return View(groupPermissionViewModel);
        }

        // GET: MenuMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int groupId = id ?? 0;
            GroupPermissionViewModel groupPermissionViewModel = new GroupPermissionViewModel();
            try
            {
                var param1 = new SqlParameter("@GroupId", groupId);
                param1.SqlDbType = System.Data.SqlDbType.Int;
                var groupPermissionEntityList = await _context.GroupPermissionEntity.FromSqlRaw("Exec [usp_GetGroupPermission] @GroupId", param1).ToListAsync().ConfigureAwait(false);

                groupPermissionViewModel.GroupId = groupPermissionEntityList[0].GroupId;
                groupPermissionViewModel.GroupName = groupPermissionEntityList[0].GroupName;
                groupPermissionViewModel.MenuIds = groupPermissionEntityList[0].MenuIds;
                groupPermissionViewModel.MenuNames = groupPermissionEntityList[0].MenuNames;
                groupPermissionViewModel.Status = groupPermissionEntityList[0].Status;
                groupPermissionViewModel.StatusName = groupPermissionEntityList[0].StatusName;
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
            return View(groupPermissionViewModel);
        }

        // GET: MenuMasters/Create
        public IActionResult Create()
        {
            GroupPermissionViewModel groupPermissionViewModel = new GroupPermissionViewModel();
            groupPermissionViewModel.lstGroup = (from grp in _context.TbGroupMasters
                                                 where grp.Status == true
                                                 select new SelectListItem()
                                                 {
                                                     Value = Convert.ToString(grp.GroupId),
                                                     Text = grp.GroupName
                                                 }).ToList();
            groupPermissionViewModel.lstMenus = (from menus in _context.TbMenuMasters
                                                 where menus.Status == true
                                                 select new SelectListItem()
                                                 {
                                                     Value = Convert.ToString(menus.MenuId),
                                                     Text = menus.Description
                                                 }).ToList();
            return View(groupPermissionViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(GroupPermissionViewModel gmVM)
        {
            try
            {
                TbGroupPermissions tbGroupPermission = new TbGroupPermissions();
                tbGroupPermission.GroupId = gmVM.GroupId;
                tbGroupPermission.MenuId = gmVM.MenuId;
                tbGroupPermission.Status = true;
                if (ModelState.IsValid)
                {
                    _context.Add(tbGroupPermission);
                    await _context.SaveChangesAsync();
                }
                TempData["SubmitResult"] = Common.MsgSaveSuccess;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(gmVM);
            }
        }

        // GET: MenuMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int groupId = id ?? 0;
            GroupPermissionViewModel groupPermissionViewModel = new GroupPermissionViewModel();
            try
            {
               
                var param1 = new SqlParameter("@GroupId", groupId);
                param1.SqlDbType = System.Data.SqlDbType.Int;
                var groupPermissionEntityList = _context.GroupPermissionEntity.FromSqlRaw("Exec [usp_GetGroupPermission] @GroupId", param1).ToList();

                //var param1 = new SqlParameter("@GroupId", id);
                //param1.SqlDbType = System.Data.SqlDbType.Int;
                //var groupPermissionEntityList =_context.GroupPermissionEntity.FromSqlRaw("Exec [usp_GetGroupPermission] @GroupId", param1).FirstOrDefault();

                groupPermissionViewModel.GroupId = groupPermissionEntityList[0].GroupId;
                groupPermissionViewModel.GroupName = groupPermissionEntityList[0].GroupName;
                groupPermissionViewModel.MenuIds = groupPermissionEntityList[0].MenuIds;
                groupPermissionViewModel.MenuNames = groupPermissionEntityList[0].MenuNames;
                groupPermissionViewModel.Status = groupPermissionEntityList[0].Status;
                groupPermissionViewModel.StatusName = groupPermissionEntityList[0].StatusName;

                List<MenusWithPermissionEntity> lstMenusWithPermissionEntity = new List<MenusWithPermissionEntity>();
                groupPermissionViewModel.lstMenusWithPermissionEntity = _context.MenusWithPermissionEntity.FromSqlRaw("Exec usp_GetMenusWithPermission @GroupId", param1).ToList();
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
            return View(groupPermissionViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GroupPermissionViewModel gmVM)
        {
            if (id != gmVM.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.TbGroupPermissions.RemoveRange(_context.TbGroupPermissions.Where(c => c.GroupId == id));
                    _context.SaveChanges();
                    if (!string.IsNullOrEmpty(gmVM.MenuIds))
                    {
                        int[] selectedMenuIds = gmVM.MenuIds.Split(',').Select(n => int.Parse(n.Trim())).ToArray();
                        if (selectedMenuIds.Length > 0)
                        {
                            List<TbGroupPermissions> gpList = new List<TbGroupPermissions>();
                            foreach (int i in selectedMenuIds)
                            {
                                TbGroupPermissions tbGroupPermission = new TbGroupPermissions();
                                tbGroupPermission.GroupId = gmVM.GroupId;
                                tbGroupPermission.MenuId = i;
                                tbGroupPermission.Status = true;
                                gpList.Add(tbGroupPermission);
                            }
                            _context.TbGroupPermissions.UpdateRange((IEnumerable<TbGroupPermission>)(IEnumerable<TbGroupPermissions>)gpList);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbGroupPermissionExists(gmVM.GroupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                int loggedInUserGroup = HttpContext.Session.GetInt32(Common.GroupId) ?? 0;
                if (gmVM.GroupId == loggedInUserGroup)
                {
                    return RedirectToAction("Index", "GroupPermissions");
                }
                else
                {
                    TempData["SubmitResult"] = Common.MsgUpdateSuccess;
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(gmVM);
        }

        // GET: MenuMasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int groupId = id ?? 0;
            GroupPermissionViewModel groupPermissionViewModel = new GroupPermissionViewModel();
            try
            {
                var param1 = new SqlParameter("@GroupId", id);
                param1.SqlDbType = System.Data.SqlDbType.Int;
                var groupPermissionEntityList = await _context.GroupPermissionEntity.FromSqlRaw("Exec [usp_GetGroupPermission] @GroupId", param1).FirstOrDefaultAsync().ConfigureAwait(false);

                groupPermissionViewModel.GroupId = groupPermissionEntityList.GroupId;
                groupPermissionViewModel.GroupName = groupPermissionEntityList.GroupName;
                groupPermissionViewModel.MenuIds = groupPermissionEntityList.MenuIds;
                groupPermissionViewModel.MenuNames = groupPermissionEntityList.MenuNames;
                groupPermissionViewModel.Status = groupPermissionEntityList.Status;
                groupPermissionViewModel.StatusName = groupPermissionEntityList.StatusName;
                groupPermissionViewModel.lstGroup = (from grp in _context.TbGroupMasters
                                                     where grp.Status == true
                                                     select new SelectListItem()
                                                     {
                                                         Value = Convert.ToString(grp.GroupId),
                                                         Text = grp.GroupName
                                                     }).ToList();
                groupPermissionViewModel.lstMenus = (from menus in _context.TbMenuMasters
                                                     where menus.Status == true
                                                     select new SelectListItem()
                                                     {
                                                         Value = Convert.ToString(menus.MenuId),
                                                         Text = menus.Description
                                                     }).ToList();
                if (!string.IsNullOrEmpty(groupPermissionViewModel.MenuIds))
                {
                    if (groupPermissionViewModel.MenuIds.Split(',').Length > 0)
                    {
                        if (groupPermissionViewModel.MenuIds.Split(',')[0] != "N/A")
                        {
                            groupPermissionViewModel.selectedMenuIds = groupPermissionViewModel.MenuIds.Split(',').Select(n => int.Parse(n.Trim())).ToArray();
                        }
                    }
                }
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
            return View(groupPermissionViewModel);
        }

        // POST: MenuMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbGroupPermission = await _context.TbGroupPermissions.FindAsync(id);
            tbGroupPermission.Status = false;
            _context.TbGroupPermissions.RemoveRange(_context.TbGroupPermissions.Where(c => c.GroupId == id));
            await _context.SaveChangesAsync();
            TempData["SubmitResult"] = Common.MsgDeleteSuccess;
            return RedirectToAction(nameof(Index));
        }

        private bool TbGroupPermissionExists(int id)
        {
            return _context.TbGroupPermissions.Any(e => e.GroupId == id);
        }
    }
}

