using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Controllers
{
    [SessionTimeoutAttribute]
    public class UserGroupMappingsController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _appcontext;
        private ApplicationConfigurations ApplicationConfigurations { get; set; }
        public UserGroupMappingsController(eSanjeevaniIcuDbContext appcontext)
        {
            _appcontext = appcontext;
        }

        // GET: UserGroupMappings
        public async Task<IActionResult> Index()
        {
            UserGroupMappingViewModel userGroupMappingViewModel = new UserGroupMappingViewModel();
            var portalUserList = new List<AllUserViewModel>();
            try
            {
                //var ideskUserList = (from ideskuser in _rccontext.IdeskUser
                //                     select new AllUserViewModel()
                //                     {
                //                         UserId = Convert.ToInt32(ideskuser.PrincipalId),
                //                         UserFullName = ideskuser.IDeskUserName + " " + ideskuser.IDeskUserLastName,
                //                     }).ToList();
                portalUserList = (from portalUser in _appcontext.TbUserMasters
                                  where portalUser.Status == true
                                  select new AllUserViewModel()
                                  {
                                      UserId = portalUser.UserId,
                                      UserFullName = portalUser.FirstName + " " + (portalUser.LastName == null ? "" : portalUser.LastName),
                                  }).ToList();
                List<AllUserViewModel> auvmList = portalUserList;
                //auvmList.AddRange(portalUserList);


                userGroupMappingViewModel.lstUserGroupMappings = await (from tbUserGroupMapping in _appcontext.TbUserGroupMappings
                                                                        join alluser in _appcontext.TbUserMasters on tbUserGroupMapping.PrincipalId equals alluser.UserId
                                                                        join tbGroupMaster in _appcontext.TbGroupMasters on tbUserGroupMapping.GroupId equals tbGroupMaster.GroupId
                                                                        where tbUserGroupMapping.Status == true && tbGroupMaster.Status == true
                                                                        select new UserGroupMappingViewModel
                                                                        {
                                                                            UserGroupMappingId = tbUserGroupMapping.UserGroupMappingId,
                                                                            PrincipalId = tbUserGroupMapping.PrincipalId,
                                                                            UserName = alluser.FirstName + " " + (alluser.LastName == null ? "" : alluser.LastName),
                                                                            GroupId = tbUserGroupMapping.GroupId,
                                                                            GroupName = tbGroupMaster.GroupName,
                                                                            Status = tbUserGroupMapping.Status,
                                                                            StatusName = tbUserGroupMapping.Status == true ? "Active" : "Inactive"
                                                                        }).ToListAsync();
            }
            catch (Exception ex)
            {
                return View(userGroupMappingViewModel);
            }
            return View(userGroupMappingViewModel);
        }

        // GET: UserGroupMappings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var ideskUserList = (from ideskuser in _rccontext.IdeskUser
            //                     select new AllUserViewModel()
            //                     {
            //                         UserId = Convert.ToInt32(ideskuser.PrincipalId),
            //                         UserFullName = ideskuser.IDeskUserName + " " + ideskuser.IDeskUserLastName,
            //                     }).ToList();
            var portalUserList = (from portalUser in _appcontext.TbUserMasters
                                  where portalUser.Status == true
                                  select new AllUserViewModel()
                                  {
                                      UserId = portalUser.UserId,
                                      UserFullName = portalUser.FirstName + " " + (portalUser.LastName == null ? "" : portalUser.LastName),
                                  }).ToList();
            List<AllUserViewModel> auvmList = portalUserList;
            //auvmList.AddRange(portalUserList);

            UserGroupMappingViewModel userGroupMappingViewModel = new UserGroupMappingViewModel();
            userGroupMappingViewModel = await (from tbUserGroupMapping in _appcontext.TbUserGroupMappings
                                               join alluser in _appcontext.TbUserMasters on tbUserGroupMapping.PrincipalId equals alluser.UserId
                                               join tbGroupMaster in _appcontext.TbGroupMasters on tbUserGroupMapping.GroupId equals tbGroupMaster.GroupId
                                               where tbUserGroupMapping.Status == true && tbUserGroupMapping.UserGroupMappingId == id && tbGroupMaster.Status == true
                                               select new UserGroupMappingViewModel
                                               {
                                                   UserGroupMappingId = tbUserGroupMapping.UserGroupMappingId,
                                                   PrincipalId = tbUserGroupMapping.PrincipalId,
                                                   UserName = alluser.FirstName + " " + (alluser.LastName == null ? "" : alluser.LastName),
                                                   GroupId = tbUserGroupMapping.GroupId,
                                                   GroupName = tbGroupMaster.GroupName,
                                                   Status = tbUserGroupMapping.Status,
                                                   StatusName = tbUserGroupMapping.Status == true ? "Active" : "Inactive"
                                               }).FirstOrDefaultAsync();
            return View(userGroupMappingViewModel);
        }

        // GET: UserGroupMappings/Create
        public IActionResult Create()
        {
            UserGroupMappingViewModel userGroupMappingViewModel = new UserGroupMappingViewModel();
            //var iDeskUserList = (from ideskuser in _rccontext.IdeskUser
            //                     join loginSecurity in _rccontext.LoginSecurityProfile on ideskuser.PrincipalId equals loginSecurity.PrincipalId
            //                     where loginSecurity.UserStatus == "A"
            //                     select new SelectListItem()
            //                     {
            //                         Value = Convert.ToString(ideskuser.PrincipalId),
            //                         Text = ideskuser.IDeskUserName + " " + ideskuser.IDeskUserLastName
            //                     }).ToList();
            var portalUserList = (from portalUser in _appcontext.TbUserMasters
                                  where portalUser.Status == true
                                  select new SelectListItem()
                                  {
                                      Value = Convert.ToString(portalUser.UserId),
                                      Text = portalUser.FirstName + " " + (portalUser.LastName == null ? "" : portalUser.LastName)
                                  }).ToList();
            //iDeskUserList.AddRange(portalUserList);
            userGroupMappingViewModel.lstUser = portalUserList.OrderBy(x => x.Text).ToList();
            userGroupMappingViewModel.lstGroup = (from groupMaster in _appcontext.TbGroupMasters
                                                  where groupMaster.Status == true
                                                  select new SelectListItem()
                                                  {
                                                      Value = Convert.ToString(groupMaster.GroupId),
                                                      Text = groupMaster.GroupName
                                                  }).ToList();
            return View(userGroupMappingViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserGroupMappingViewModel ugmVM)
        {
            try
            {
                TbUserGroupMapping tbUserGroupMapping = new TbUserGroupMapping();
                tbUserGroupMapping.PrincipalId = ugmVM.PrincipalId;
                tbUserGroupMapping.GroupId = ugmVM.GroupId;
                tbUserGroupMapping.Status = true;
                if (ModelState.IsValid)
                {
                    _appcontext.Add(tbUserGroupMapping);
                    await _appcontext.SaveChangesAsync();
                    TempData["SubmitResult"] = Common.MsgSaveSuccess;
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //var iDeskUserList = (from ideskuser in _rccontext.IdeskUser
                //                     select new SelectListItem()
                //                     {
                //                         Value = Convert.ToString(ideskuser.PrincipalId),
                //                         Text = ideskuser.IDeskUserName + " " + ideskuser.IDeskUserLastName
                //                     }).ToList();
                var portalUserList = (from portalUser in _appcontext.TbUserMasters
                                      where portalUser.Status == true
                                      select new SelectListItem()
                                      {
                                          Value = Convert.ToString(portalUser.UserId),
                                          Text = portalUser.FirstName + " " + (portalUser.LastName == null ? "" : portalUser.LastName)
                                      }).ToList();
                ugmVM.lstUser = portalUserList;
                ugmVM.lstUser.AddRange(portalUserList);
                ugmVM.lstGroup = (from groupMaster in _appcontext.TbGroupMasters
                                  where groupMaster.Status == true
                                  select new SelectListItem()
                                  {
                                      Value = Convert.ToString(groupMaster.GroupId),
                                      Text = groupMaster.GroupName
                                  }).ToList();
                return View(ugmVM);
            }
        }

        // GET: UserGroupMappings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var ideskUserList = (from ideskuser in _rccontext.IdeskUser
            //                     join loginSecurity in _rccontext.LoginSecurityProfile on ideskuser.PrincipalId equals loginSecurity.PrincipalId
            //                     where loginSecurity.UserStatus == "A"
            //                     select new AllUserViewModel()
            //                     {
            //                         UserId = Convert.ToInt32(ideskuser.PrincipalId),
            //                         UserFullName = ideskuser.IDeskUserName + " " + ideskuser.IDeskUserLastName,
            //                     }).ToList();
            var portalUserList = (from portalUser in _appcontext.TbUserMasters
                                  where portalUser.Status == true
                                  select new AllUserViewModel()
                                  {
                                      UserId = portalUser.UserId,
                                      UserFullName = portalUser.FirstName + " " + (portalUser.LastName == null ? "" : portalUser.LastName)
                                  }).ToList();
            List<AllUserViewModel> auvmList = portalUserList;
            //auvmList.AddRange(portalUserList);

            UserGroupMappingViewModel userGroupMappingViewModel = new UserGroupMappingViewModel();
            userGroupMappingViewModel = await (from tbUserGroupMapping in _appcontext.TbUserGroupMappings
                                               join alluser in _appcontext.TbUserMasters on tbUserGroupMapping.PrincipalId equals alluser.UserId
                                               join tbGroupMaster in _appcontext.TbGroupMasters on tbUserGroupMapping.GroupId equals tbGroupMaster.GroupId
                                               where tbUserGroupMapping.Status == true && tbUserGroupMapping.UserGroupMappingId == id && tbGroupMaster.Status == true
                                               select new UserGroupMappingViewModel
                                               {
                                                   UserGroupMappingId = tbUserGroupMapping.UserGroupMappingId,
                                                   PrincipalId = tbUserGroupMapping.PrincipalId,
                                                   UserName = alluser.FirstName + " " + (alluser.LastName == null ? "" : alluser.LastName),
                                                   GroupId = tbUserGroupMapping.GroupId,
                                                   GroupName = tbGroupMaster.GroupName,
                                                   Status = tbUserGroupMapping.Status,
                                                   StatusName = tbUserGroupMapping.Status == true ? "Active" : "Inactive"
                                               }).FirstOrDefaultAsync();

            userGroupMappingViewModel.lstUser = (from ideskuser in auvmList
                                                 select new SelectListItem()
                                                 {
                                                     Value = Convert.ToString(ideskuser.UserId),
                                                     Text = ideskuser.UserFullName
                                                 }).OrderBy(x => x.Text).ToList();
            userGroupMappingViewModel.lstGroup = (from groupMaster in _appcontext.TbGroupMasters
                                                  where groupMaster.Status == true
                                                  select new SelectListItem()
                                                  {
                                                      Value = Convert.ToString(groupMaster.GroupId),
                                                      Text = groupMaster.GroupName
                                                  }).ToList();

            if (userGroupMappingViewModel == null)
            {
                return NotFound();
            }
            return View(userGroupMappingViewModel);
        }

        // POST: UserGroupMappings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserGroupMappingViewModel ugmVM)
        {
            if (id != ugmVM.UserGroupMappingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TbUserGroupMapping tbUserGroupMapping = new TbUserGroupMapping();
                    tbUserGroupMapping.UserGroupMappingId = ugmVM.UserGroupMappingId;
                    tbUserGroupMapping.PrincipalId = ugmVM.PrincipalId;
                    tbUserGroupMapping.GroupId = ugmVM.GroupId;
                    tbUserGroupMapping.Status = true;
                    _appcontext.Update(tbUserGroupMapping);
                    await _appcontext.SaveChangesAsync();
                    TempData["SubmitResult"] = Common.MsgUpdateSuccess;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbUserGroupMappingExists(ugmVM.UserGroupMappingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                int loggedInUser = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                if (ugmVM.PrincipalId == loggedInUser)
                {
                    return RedirectToAction("Index", "OnboardingForm");
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Edit), ugmVM.UserGroupMappingId);
        }


        // GET: UserGroupMappings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var ideskUserList = (from ideskuser in _rccontext.IdeskUser
            //                     select new AllUserViewModel()
            //                     {
            //                         UserId = Convert.ToInt32(ideskuser.PrincipalId),
            //                         UserFullName = ideskuser.IDeskUserName + " " + ideskuser.IDeskUserLastName,
            //                     }).ToList();
            var portalUserList = (from portalUser in _appcontext.TbUserMasters
                                  where portalUser.Status == true
                                  select new AllUserViewModel()
                                  {
                                      UserId = portalUser.UserId,
                                      UserFullName = portalUser.FirstName + " " + (portalUser.LastName == null ? "" : portalUser.LastName),
                                  }).ToList();
            List<AllUserViewModel> auvmList = portalUserList;
            //auvmList.AddRange(portalUserList);

            UserGroupMappingViewModel userGroupMappingViewModel = new UserGroupMappingViewModel();
            userGroupMappingViewModel = await (from tbUserGroupMapping in _appcontext.TbUserGroupMappings
                                               join alluser in _appcontext.TbUserMasters on tbUserGroupMapping.PrincipalId equals alluser.UserId
                                               join tbGroupMaster in _appcontext.TbGroupMasters on tbUserGroupMapping.GroupId equals tbGroupMaster.GroupId
                                               where tbUserGroupMapping.Status == true && tbUserGroupMapping.UserGroupMappingId == id && tbGroupMaster.Status == true
                                               select new UserGroupMappingViewModel
                                               {
                                                   UserGroupMappingId = tbUserGroupMapping.UserGroupMappingId,
                                                   PrincipalId = tbUserGroupMapping.PrincipalId,
                                                   UserName = alluser.FirstName + " " + (alluser.LastName == null ? "" : alluser.LastName),
                                                   GroupId = tbUserGroupMapping.GroupId,
                                                   GroupName = tbGroupMaster.GroupName,
                                                   Status = tbUserGroupMapping.Status,
                                                   StatusName = tbUserGroupMapping.Status == true ? "Active" : "Inactive"
                                               }).FirstOrDefaultAsync();
            return View(userGroupMappingViewModel);
        }

        // POST: UserGroupMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbUserGroupMapping = await _appcontext.TbUserGroupMappings.FindAsync(id);
            _appcontext.TbUserGroupMappings.Remove(tbUserGroupMapping);
            await _appcontext.SaveChangesAsync();
            TempData["SubmitResult"] = Common.MsgDeleteSuccess;
            return RedirectToAction(nameof(Index));
        }

        private bool TbUserGroupMappingExists(int id)
        {
            return _appcontext.TbUserGroupMappings.Any(e => e.UserGroupMappingId == id);
        }
    }
}
