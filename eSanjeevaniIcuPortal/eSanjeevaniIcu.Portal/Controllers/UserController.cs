using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Portal.Models;
using eSanjeevaniIcu.Shared.Enum;
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

namespace eSanjeevaniIcu.Portal.Controllers
{
    [SessionTimeoutAttribute]
    public class UserController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;
        private readonly CommonController _commonController;
        private readonly CommonFunctions _commonFunctions;
        private EmailConfiguration EmailConfigurations { get; set; }
        private ApplicationConfigurations ApplicationConfigurations { get; set; }
        public UserController(eSanjeevaniIcuDbContext context, IOptions<ApplicationConfigurations> settings, IOptions<EmailConfiguration> emailSettings)
        {
            _context = context;
            _commonController = new CommonController(_context, settings, emailSettings);
            EmailConfigurations = emailSettings.Value;
            ApplicationConfigurations = settings.Value;
            _commonFunctions = new CommonFunctions(_context);
        }

        public ActionResult Index()
        {
            UserViewModel UserViewModel = new UserViewModel();
            try
            {
                UserViewModel.lstUserViewModel = (from userMaster in _context.TbUserMasters
                                                  join ut in _context.TbGroupMasters.DefaultIfEmpty() on userMaster.UserTypeId equals ut.GroupId
                                                  //join sm in _context.TbStatusMaster.DefaultIfEmpty() on userMaster.Status equals sm.StatusId
                                                  // where userMaster.Status == true
                                                  select new UserViewModel
                                                  {
                                                      UserId = userMaster.UserId,
                                                      FirstName = userMaster.FirstName,
                                                      LastName = userMaster.LastName,
                                                      UserName = userMaster.UserName,
                                                      //Password = userMaster.Password,
                                                      UserTypeId = userMaster.UserTypeId,
                                                      UserType = ut.GroupName,
                                                      MobileNumber = userMaster.MobileNumber,
                                                      Email = userMaster.Email,
                                                      Status = userMaster.Status,
                                                      StatusName = userMaster.Status == true ? "Active" : "Inactive"
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
            return View(UserViewModel);
        }

        [HttpGet]
        public ActionResult GetSpecialistStatusList()
        {
            UserViewModel UserViewModel = new UserViewModel();
            try
            {
                UserViewModel.lstUserViewModel = (from userMaster in _context.TbUserMasters
                                                  join gm in _context.TbGroupMasters.DefaultIfEmpty() on userMaster.UserTypeId equals gm.GroupId
                                                  join userActivity in _context.TbUserActivity.DefaultIfEmpty() on userMaster.UserId equals userActivity.UserId
                                                  where userMaster.Status == true && userActivity.Status == true && gm.GroupId == Common.SpecialistGroupId
                                                  select new UserViewModel
                                                  {
                                                      UserId = userMaster.UserId,
                                                      FirstName = userMaster.FirstName +" "+ userMaster.LastName,
                                                      MobileNumber = userMaster.MobileNumber,
                                                      StatusName = userActivity.IsLogin == true ? "1" : "2",
                                                  }).ToList();
                return Json(new { data = UserViewModel.lstUserViewModel});
            }
            catch (Exception ex)
            {
                return Json(new { data = new UserViewModel()});
            }
        }
        DateTime dat = new DateTime();
        public string GetStatus(DateTime LoginTime, bool IsLogin)
        {
            string status = "2";
            if(LoginTime != DateTime.MinValue)
            {
                if (IsLogin)
                {
                    status = "1";
                }
                else
                {
                    status = "2";
                }
            }
            else
            {
                status = "2";
            }
            return status;
        }
        public ActionResult UserProfile()
        {
            try
            {
                string username = HttpContext.Session.GetString(Common.UserId);

                var userViewModel = (from userMaster in _context.TbUserMasters
                                     where userMaster.UserName == username
                                     select new UserViewModel
                                     {
                                         UserId = userMaster.UserId,
                                         FirstName = userMaster.FirstName,
                                         LastName = userMaster.LastName,
                                         UserName = userMaster.UserName,
                                         UserTypeId = userMaster.UserTypeId,
                                         MobileNumber = userMaster.MobileNumber,
                                         Email = userMaster.Email,
                                         Status = userMaster.Status,
                                         StatusName = userMaster.Status ? "Active" : "Inactive"
                                     })
                    .FirstOrDefault();

                if (userViewModel == null)
                {
                    return NotFound();
                }

                return View(userViewModel);
            }
            catch (SqlException ex)
            {
                return StatusCode(500);
            }
            
        }








        // GET: PortalUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                UserViewModel userViewModel = (from userMaster in _context.TbUserMasters
                                               join ut in _context.TbGroupMasters.DefaultIfEmpty() on userMaster.UserTypeId equals ut.GroupId
                                               //join sm in _context.TbStatusMaster.DefaultIfEmpty() on userMaster.Status equals sm.StatusId                                               
                                               where userMaster.UserId == id
                                               select new UserViewModel
                                               {
                                                   UserId = userMaster.UserId,
                                                   FirstName = userMaster.FirstName,
                                                   LastName = userMaster.LastName,
                                                   UserName = userMaster.UserName,
                                                   Password = userMaster.Password,
                                                   UserTypeId = userMaster.UserTypeId,
                                                   UserType = ut.GroupName,
                                                   MobileNumber = userMaster.MobileNumber,
                                                   Email = userMaster.Email,
                                                   Status = userMaster.Status,
                                                   StatusName = string.Empty,//sm.StatusName
                                               }).FirstOrDefault();

                return View(userViewModel);
            }
            catch (Exception ex)
            {
                string result = ex.ToString();
                UserViewModel userViewModel = new UserViewModel();
                return View(userViewModel);
            }
        }

        // GET: PortalUser/Create
        public IActionResult Create()
        {
            UserViewModel UserViewModel = new UserViewModel();

            var userTypeList = new List<SelectListItem>();
            userTypeList = (from t1 in _context.TbGroupMasters
                            where t1.Status == true
                            select new SelectListItem
                            {
                                Value = Convert.ToString(t1.GroupId),
                                Text = t1.GroupName
                            }).OrderBy(x => x.Text).ToList();
            UserViewModel.lstUserType = userTypeList;
            //TO DO : RAVI
            //UserViewModel.Password = RandomPasswordGenerator.generatePassword(6, true);
            return View(UserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel sdcVM)
        {
            try
            {
                if (TbSdcUserNameExists(sdcVM.UserName))
                {
                    ViewBag.UserNameAvailability = "User name already exist.";

                    var userTypeList = new List<SelectListItem>();
                    userTypeList = (from t1 in _context.TbGroupMasters
                                    where t1.Status == true
                                    select new SelectListItem
                                    {
                                        Value = Convert.ToString(t1.GroupId),
                                        Text = t1.GroupName
                                    }).OrderBy(x => x.Text).ToList();
                    sdcVM.lstUserType = userTypeList;
                }
                int principalId = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                int groupId = HttpContext.Session.GetInt32(Common.GroupId) ?? 0;
                //TO DO : RAVI
                //string newPass = RandomPasswordGenerator.generatePassword(6, true);
                string newPass = sdcVM.Password == null ? "pass1234" : sdcVM.Password;
                TbUserMaster tbPortalUserMaster = new TbUserMaster();
                tbPortalUserMaster.UserTypeId = sdcVM.UserTypeId;
                tbPortalUserMaster.FirstName = sdcVM.FirstName;
                tbPortalUserMaster.LastName = sdcVM.LastName;
                tbPortalUserMaster.UserName = sdcVM.UserName;
                tbPortalUserMaster.Password = RandomPasswordGenerator.encode(newPass.Trim());
                tbPortalUserMaster.MobileNumber = sdcVM.MobileNumber;
                tbPortalUserMaster.Email = sdcVM.Email;

                tbPortalUserMaster.CreatedOn = DateTime.Now;
                tbPortalUserMaster.CreatedBy = principalId;
                if (groupId == (Int32)UserGroupEnum.SuperUser || groupId == (Int32)UserGroupEnum.Admin)
                {
                    tbPortalUserMaster.Status = true;
                }
                else
                {
                    tbPortalUserMaster.Status = sdcVM.StatusId == 1 ? true : false;
                }
                if (ModelState.IsValid)
                {
                    _context.Add(tbPortalUserMaster);
                    await _context.SaveChangesAsync();

                    int userId = tbPortalUserMaster.UserId;
                    TbUserGroupMapping tbUserGroupMapping = new TbUserGroupMapping();
                    tbUserGroupMapping.PrincipalId = userId;
                    tbUserGroupMapping.GroupId = sdcVM.UserTypeId;
                    tbUserGroupMapping.Status = true;
                    _context.Add(tbUserGroupMapping);
                    await _context.SaveChangesAsync();

                    string htmlString = @"<html>
                      <body>
                      <p>Hi</p>
                      <p>Your temporary Password to access Bullant Creative Portal for User : @UserName</p>
                      <p> is : @Password</p>
                      <p>Thanks,<br>Admin</br></p>
                      </body>
                      </html>
                     ";
                    htmlString = htmlString.Replace("@UserName", sdcVM.UserName);
                    htmlString = htmlString.Replace("@Password", newPass);

                    bool result = SendEmail("Bullant Creative Portal Credential", htmlString, sdcVM.Email);
                }
                TempData["SubmitResult"] = Common.MsgSaveSuccess;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var stateList = new List<SelectListItem>();
                var userTypeList = new List<SelectListItem>();
                userTypeList = (from t1 in _context.TbGroupMasters
                                where t1.Status == true
                                select new SelectListItem
                                {
                                    Value = Convert.ToString(t1.GroupId),
                                    Text = t1.GroupName
                                }).OrderBy(x => x.Text).ToList();
                sdcVM.lstUserType = userTypeList;
                return View(sdcVM);
            }
        }

        public bool SendEmail(string subject, string body, string mailto)
        {
            try
            {
                string strFrom = EmailConfigurations.SmtpUsername;
                string strDisplayName = EmailConfigurations.SmtpDisplayname;
                string strCC = EmailConfigurations.DefaultCC;

                MailMessage mail = new MailMessage();
                mail.To.Add(mailto);
                mail.From = new MailAddress(strFrom, strDisplayName);
                if (strCC != "")
                {
                    mail.CC.Add(strCC);
                }
                mail.Subject = subject;
                mail.Body = body;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = EmailConfigurations.SmtpServer;
                smtp.Port = EmailConfigurations.SmtpPort;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = EmailConfigurations.EnableSsl;
                smtp.Credentials = new NetworkCredential(EmailConfigurations.SmtpUsername, EmailConfigurations.SmtpPassword);
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public ActionResult EmailPassword(int? id)
        {
            try
            {
                int principalId = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                TbUserMaster tbPortalUserMaster = new TbUserMaster();
                tbPortalUserMaster = (from portalUserMaster in _context.TbUserMasters
                                      where portalUserMaster.UserId == id && portalUserMaster.Status == true
                                      select portalUserMaster).FirstOrDefault();
                string newPass = RandomPasswordGenerator.generatePassword(6, true);
                tbPortalUserMaster.Password = RandomPasswordGenerator.encode(newPass.Trim());
                tbPortalUserMaster.LastUpdatedOn = DateTime.Now;
                tbPortalUserMaster.LastUpdatedBy = principalId;
                tbPortalUserMaster.Status = true;
                _context.Update(tbPortalUserMaster);
                _context.SaveChanges();

                string htmlString = @"<html>
                      <body>
                      <p>Hi</p>
                      <p>Below is the link & credential to access Bullant Creative Web Portal.</p>
                      <p><a href='@WebLink' target='_blank'>@WebLink</a></p>
                      <p>Username : @UserName</p>
                      <p> Password : @Password</p>
                      <p>Thanks,<br>Admin</br></p>
                      </body>
                      </html>
                     ";
                htmlString = htmlString.Replace("@WebLink", ApplicationConfigurations.ApplicationHostUrl);
                htmlString = htmlString.Replace("@UserName", tbPortalUserMaster.UserName);
                htmlString = htmlString.Replace("@Password", newPass);

                bool result = SendEmail("Bullant Creative Credential", htmlString, tbPortalUserMaster.Email);
                if (result == true)
                {
                    //ViewBag.PasswordMessage = "PasswordSendSuccessfully";
                    TempData["PasswordMessage"] = "PasswordSendSuccessfully";
                }
                else
                {
                    //ViewBag.PasswordMessage = "PasswordSendFailed";
                    TempData["PasswordMessage"] = "PasswordSendFailed";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.PasswordMessage = "PasswordSendFailed";
                TempData["PasswordMessage"] = "PasswordSendFailed";
                return RedirectToAction(nameof(Index));
            }
        }
        // GET: PortalUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbPortalUserMaster = await _context.TbUserMasters.FindAsync(id);
            if (tbPortalUserMaster == null)
            {
                return NotFound();
            }
            UserViewModel UserViewModel = new UserViewModel();
            UserViewModel = (from userMaster in _context.TbUserMasters
                             join ut in _context.TbGroupMasters.DefaultIfEmpty() on userMaster.UserTypeId equals ut.GroupId
                             //join sm in _context.TbStatusMaster.DefaultIfEmpty() on userMaster.Status equals sm.StatusId
                             where userMaster.UserId == id
                             select new UserViewModel
                             {
                                 UserId = userMaster.UserId,
                                 UserTypeId = userMaster.UserTypeId,
                                 UserType = ut.GroupName,
                                 FirstName = userMaster.FirstName,
                                 LastName = userMaster.LastName,
                                 UserName = userMaster.UserName,
                                 MobileNumber = userMaster.MobileNumber,
                                 Email = userMaster.Email,
                                 //Password = userMaster.Password.Trim(),
                                 Status = userMaster.Status,
                                 StatusName = string.Empty,//sm.StatusName,
                                 CreatedOn = userMaster.CreatedOn,
                                 CreatedBy = userMaster.CreatedBy,
                                 GroupId = ut.GroupId,
                                 StatusId = userMaster.Status == true ? 1 : 0,
                             }).FirstOrDefault();

            var userTypeList = new List<SelectListItem>();
            userTypeList = (from t1 in _context.TbGroupMasters
                            where t1.Status == true
                            select new SelectListItem
                            {
                                Value = Convert.ToString(t1.GroupId),
                                Text = t1.GroupName
                            }).OrderBy(x => x.Text).ToList();



            UserViewModel.lstUserType = userTypeList;
            int groupId = HttpContext.Session.GetInt32(Common.GroupId) ?? 0;
            UserViewModel.GroupId = groupId;
            return View(UserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel sdcVM)
        {
            if (id != sdcVM.UserId)
            {
                return View(sdcVM);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var tbPortalUserMaster = await _context.TbUserMasters.FindAsync(id);
                    if (tbPortalUserMaster == null)
                    {
                        return NotFound();
                    }
                    int principalId = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                    tbPortalUserMaster.UserId = id;
                    tbPortalUserMaster.UserTypeId = sdcVM.UserTypeId;
                    tbPortalUserMaster.FirstName = sdcVM.FirstName;
                    tbPortalUserMaster.LastName = sdcVM.LastName;
                    tbPortalUserMaster.MobileNumber = sdcVM.MobileNumber;
                    tbPortalUserMaster.Email = sdcVM.Email;

                    tbPortalUserMaster.LastUpdatedOn = DateTime.Now;
                    tbPortalUserMaster.LastUpdatedBy = principalId;
                    tbPortalUserMaster.Status = sdcVM.StatusId == 1 ? true : false;
                    if (sdcVM.Password != null)
                    {
                        tbPortalUserMaster.Password = RandomPasswordGenerator.encode(sdcVM.Password.Trim());
                    }
                    _context.Update(tbPortalUserMaster);
                    await _context.SaveChangesAsync();

                    int userId = tbPortalUserMaster.UserId;
                    var tbUserGroupMapping = (from userGroupMapping in _context.TbUserGroupMappings
                                              where userGroupMapping.PrincipalId == userId
                                              select userGroupMapping).FirstOrDefault();
                    if (tbUserGroupMapping == null)
                    {
                        tbUserGroupMapping = new TbUserGroupMapping();
                        tbUserGroupMapping.PrincipalId = userId;
                        tbUserGroupMapping.GroupId = sdcVM.UserTypeId;
                        tbUserGroupMapping.Status = false;
                        _context.Add(tbUserGroupMapping);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        tbUserGroupMapping.UserGroupMappingId = tbUserGroupMapping.UserGroupMappingId;
                        tbUserGroupMapping.PrincipalId = userId;
                        tbUserGroupMapping.GroupId = sdcVM.UserTypeId;
                        tbUserGroupMapping.Status = true;
                        _context.Update(tbUserGroupMapping);
                        await _context.SaveChangesAsync();
                    }
                    TempData["SubmitResult"] = Common.MsgUpdateSuccess;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var userTypeList = new List<SelectListItem>();
                    userTypeList = (from t1 in _context.TbGroupMasters
                                    where t1.Status == true
                                    select new SelectListItem
                                    {
                                        Value = Convert.ToString(t1.GroupId),
                                        Text = t1.GroupName
                                    }).OrderBy(x => x.Text).ToList();
                    sdcVM.lstUserType = userTypeList;
                    return View(sdcVM);
                }
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!TbPortalUserMasterExists(sdcVM.UserId))
                {
                    return View(sdcVM);
                }
                else
                {
                    return View(sdcVM);
                }
            }
        }

        // GET: PortalUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbPortalUserMaster = await _context.TbUserMasters
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (tbPortalUserMaster == null)
            {
                return NotFound();
            }
            UserViewModel UserViewModel = new UserViewModel();
            UserViewModel = (from userMaster in _context.TbUserMasters
                             join ut in _context.TbGroupMasters.DefaultIfEmpty() on userMaster.UserTypeId equals ut.GroupId
                             where userMaster.UserId == id
                             select new UserViewModel
                             {
                                 UserId = userMaster.UserId,
                                 UserTypeId = userMaster.UserTypeId,
                                 UserType = ut.GroupName,
                                 FirstName = userMaster.FirstName,
                                 LastName = userMaster.LastName,
                                 UserName = userMaster.UserName,
                                 MobileNumber = userMaster.MobileNumber,
                                 Email = userMaster.Email,
                                 Password = userMaster.Password.Trim(),
                                 Status = userMaster.Status,
                                 StatusName = string.Empty
                             }).FirstOrDefault();
            return View(UserViewModel);
        }

        // POST: PortalUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbPortalUserMaster = await _context.TbUserMasters.FindAsync(id);
            tbPortalUserMaster.Status = false;
            _context.Update(tbPortalUserMaster);
            await _context.SaveChangesAsync();
            var tbUserGroupMapping = await (from tbUserGroup in _context.TbUserGroupMappings
                                            where tbUserGroup.PrincipalId == tbPortalUserMaster.UserId && tbUserGroup.Status == true
                                            select tbUserGroup).FirstOrDefaultAsync();
            if (tbUserGroupMapping != null)
            {
                tbUserGroupMapping.Status = false;
                _context.Update(tbUserGroupMapping);
            }
            await _context.SaveChangesAsync();
            TempData["SubmitResult"] = Common.MsgDeleteSuccess;
            return RedirectToAction(nameof(Index));
        }

        private bool TbPortalUserMasterExists(int id)
        {
            return _context.TbUserMasters.Any(e => e.UserId == id);
        }
        private bool TbSdcUserNameExists(string userName)
        {
            return _context.TbUserMasters.Any(e => e.UserName == userName);
        }

        [HttpPost]
        public JsonResult IsEmailExist(string emailId, int? userid)
        {
            bool isExistEmail = false;
            try
            {
                if (userid != null && userid != 0)
                {
                    string oldEmailId = (from tbUser in _context.TbUserMasters
                                         where tbUser.Status == true && tbUser.UserId == userid
                                         select tbUser).FirstOrDefault().Email;
                    if (oldEmailId != emailId)
                    {
                        isExistEmail = (_context.TbUserMasters.Where(x => x.Status == true && x.Email == emailId && x.UserId != userid).Count() > 0);
                    }

                }
                else
                {
                    isExistEmail = (_context.TbUserMasters.Where(x => x.Status == true && x.Email == emailId).Count() > 0);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, isExistEmail, responseText = "Record Don't Retrieved Successfully!" });
            }
            return Json(new { success = true, isExistEmail, responseText = "Record Retrieved Successfully!" });
        }


    }
}
