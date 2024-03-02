using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Controllers
{
    [SessionTimeoutAttribute]
    public class GroupMastersController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;

        public GroupMastersController(eSanjeevaniIcuDbContext context)
        {
            _context = context;
        }

        // GET: MenuMasters
        public ActionResult Index()
        {
            GroupMasterViewModel groupMasterViewModel = new GroupMasterViewModel();
            try
            {
                var tbGroupMaster = (from tbGroup in _context.TbGroupMasters
                                                       where tbGroup.Status == true
                                                       select tbGroup).ToList();
                groupMasterViewModel.lstGroupMaster = (from tbGroupMas in tbGroupMaster
                                                       select new GroupMasterViewModel
                                                       {
                                                           GroupId = tbGroupMas.GroupId,
                                                           GroupName = tbGroupMas.GroupName,
                                                           Status = tbGroupMas.Status,
                                                           StatusName = tbGroupMas.Status == true ? "Active" : "Inactive",
                                                           IsDeletable = IsGroupDeletable(tbGroupMas.GroupId)
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
            return View(groupMasterViewModel);
        }

        // GET: MenuMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGroupMaster = await _context.TbGroupMasters
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (tbGroupMaster == null)
            {
                return NotFound();
            }
            GroupMasterViewModel groupMasterViewModel = new GroupMasterViewModel();
            groupMasterViewModel.GroupId = tbGroupMaster.GroupId;
            groupMasterViewModel.GroupName = tbGroupMaster.GroupName;
            groupMasterViewModel.Status = tbGroupMaster.Status;
            groupMasterViewModel.StatusName = tbGroupMaster.Status == true ? "Active" : "Inactive";
            return View(groupMasterViewModel);
        }

        // GET: MenuMasters/Create
        public IActionResult Create()
        {
            GroupMasterViewModel groupMasterViewModel = new GroupMasterViewModel();
            return View(groupMasterViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(GroupMasterViewModel gmVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TbGroupMaster tbGroupMaster = new TbGroupMaster();
                    tbGroupMaster.GroupName = gmVM.GroupName;
                    tbGroupMaster.Status = true;
                    if (ModelState.IsValid)
                    {
                        _context.Add(tbGroupMaster);
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
            return View(gmVM);
        }

        // GET: MenuMasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGroupMaster = await _context.TbGroupMasters.FindAsync(id);
            if (tbGroupMaster == null)
            {
                return NotFound();
            }
            else
            {
                GroupMasterViewModel groupMasterViewModel = new GroupMasterViewModel();
                groupMasterViewModel.GroupId = tbGroupMaster.GroupId;
                groupMasterViewModel.GroupName = tbGroupMaster.GroupName;
                groupMasterViewModel.Status = tbGroupMaster.Status;
                groupMasterViewModel.StatusName = tbGroupMaster.Status == true ? "Active" : "Inactive";
                return View(groupMasterViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GroupMasterViewModel gmVM)
        {
            if (id != gmVM.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TbGroupMaster tbGroupMaster = new TbGroupMaster();
                    tbGroupMaster.GroupId = gmVM.GroupId;
                    tbGroupMaster.GroupName = gmVM.GroupName;
                    tbGroupMaster.Status = true;
                    _context.Update(tbGroupMaster);
                    await _context.SaveChangesAsync();
                    TempData["SubmitResult"] = Common.MsgUpdateSuccess;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbGroupMasterExists(gmVM.GroupId))
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
            return View(gmVM);
        }

        // GET: MenuMasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GroupMasterViewModel groupMasterViewModel = new GroupMasterViewModel();
            var tbGroupMaster = await _context.TbGroupMasters
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (tbGroupMaster == null)
            {
                return NotFound();
            }
            groupMasterViewModel.GroupId = tbGroupMaster.GroupId;
            groupMasterViewModel.GroupName = tbGroupMaster.GroupName;            
            groupMasterViewModel.Status = tbGroupMaster.Status;
            groupMasterViewModel.StatusName = tbGroupMaster.Status == true ? "Active" : "Inactive";
            return View(groupMasterViewModel);
        }

        // POST: MenuMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbGroupMaster = await _context.TbGroupMasters.FindAsync(id);
            tbGroupMaster.Status = false;
            _context.Update(tbGroupMaster);
            await _context.SaveChangesAsync();
            TempData["SubmitResult"] = Common.MsgDeleteSuccess;
            return RedirectToAction(nameof(Index));
        }
        private bool IsGroupDeletable(int groupId)
        {
            var tbUserGroupMapping = (from tbUserGroup in _context.TbUserGroupMappings
                                      where tbUserGroup.GroupId == groupId && tbUserGroup.Status == true
                                      select tbUserGroup).ToList();
            bool result = false;
            if (tbUserGroupMapping != null && tbUserGroupMapping.Count > 0)
            {
                result = true;
            }
            return result;
        }

        private bool TbGroupMasterExists(int id)
        {
            return _context.TbGroupMasters.Any(e => e.GroupId == id);
        }
    }
}
