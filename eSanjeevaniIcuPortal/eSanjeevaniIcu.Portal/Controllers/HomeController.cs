using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Data.ExtendedEntities;
using eSanjeevaniIcu.Portal.Controllers;
using eSanjeevaniIcu.Portal.Models;
using eSanjeevaniIcu.Shared.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SecurityGeneratePwd;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using WebPortal.Models;

namespace eSanjeevaniIcu.Portal
{
    public class HomeController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _appcontext;
        private readonly CommonController _commonController;
        private readonly CommonFunctions _commonFunctions;
        private EmailConfiguration EmailConfigurations { get; set; }
        private ApplicationConfigurations ApplicationConfigurations { get; set; }
        public HomeController(eSanjeevaniIcuDbContext rcAppDbContext, IOptions<ApplicationConfigurations> settings, IOptions<EmailConfiguration> emailSettings)
        {
            _appcontext = rcAppDbContext;
            _commonFunctions = new CommonFunctions(_appcontext);
            _commonController = new CommonController(_appcontext, settings, emailSettings);
            EmailConfigurations = emailSettings.Value;
            ApplicationConfigurations = settings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult Logout()
        {
            TbUserActivity ua = new TbUserActivity();
            ua = (from t1 in _appcontext.TbUserActivity
                  where t1.Status == true && t1.UserId == HttpContext.Session.GetInt32(Common.PrincipalId) 
                  select new TbUserActivity
                  {
                      UserActivityId = t1.UserActivityId,
                      UserId = t1.UserId,
                      UserTypeId = t1.UserTypeId,
                      LoginTime = t1.LoginTime,
                      LogoutTime = t1.LogoutTime,
                      IsLogin = t1.IsLogin,
                      Status = t1.Status,
                      CreatedOn = t1.CreatedOn,
                      UpdatedOn = t1.UpdatedOn,

                  }).FirstOrDefault();
            ua.LogoutTime = DateTime.UtcNow;
            ua.IsLogin = false;
            ua.UpdatedOn = DateTime.UtcNow;
            _appcontext.Update(ua);
            _appcontext.SaveChanges();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult LogIn(AllUserViewModel objLogIn)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userData = Validate(objLogIn.UserName, objLogIn.Password);
                    if (userData != null && userData.UserName != null)
                    {
                        HttpContext.Session.SetString(Common.UserId, userData.UserName);
                        HttpContext.Session.SetString(Common.EmailId, userData.Email);
                        HttpContext.Session.SetString(Common.LoggedInUserName, userData.UserFullName);
                        int principalId = Convert.ToInt32(userData.UserId);
                        HttpContext.Session.SetInt32(Common.PrincipalId, principalId);
                        string OperatorName = userData.UserFullName;
                        if (string.IsNullOrWhiteSpace(OperatorName))
                        {
                            OperatorName = string.Empty;
                        }

                        int assGroupId = (from groupMapping in _appcontext.TbUserGroupMappings
                                          where groupMapping.PrincipalId == principalId && groupMapping.Status == true
                                          select groupMapping.GroupId).FirstOrDefault();

                        int groupId = Convert.ToInt32(assGroupId);
                        HttpContext.Session.SetInt32(Common.GroupId, groupId);
                        //---To create user activity 
                        TbUserActivity ua = new TbUserActivity();
                        if (TbUserActivityUserIdExists(userData.UserId))
                        {
                            ua = (from t1 in _appcontext.TbUserActivity
                                  where t1.Status == true && t1.UserId == userData.UserId
                                  select new TbUserActivity
                                  {
                                      UserActivityId = t1.UserActivityId,
                                      UserId = t1.UserId,
                                      UserTypeId = t1.UserTypeId,
                                      LoginTime = t1.LoginTime,
                                      LogoutTime = t1.LogoutTime,
                                      IsLogin = t1.IsLogin,
                                      Status = t1.Status,
                                      CreatedOn = t1.CreatedOn,
                                      UpdatedOn = t1.UpdatedOn,

                                  }).FirstOrDefault();
                            ua.LoginTime = DateTime.UtcNow;
                            ua.IsLogin = true;
                            ua.UpdatedOn = DateTime.UtcNow;
                            _appcontext.Update(ua);
                            _appcontext.SaveChanges();
                        }
                    
                    else
                    {
                        ua.UserId = userData.UserId;
                        ua.UserTypeId = userData.UserTypeId;
                        ua.LoginTime = DateTime.UtcNow;
                            ua.IsLogin = true;
                            ua.CreatedOn = DateTime.UtcNow;
                        _appcontext.Add(ua);
                        _appcontext.SaveChanges();
                    }
                    //---To create user activity 
                    return RedirectToAction("Index", "DashBoard");
                }
                else
                {
                    ViewBag.LoginMessage = "NotAuthenticated";
                    return View(objLogIn);
                }
            }
                else
            {
                ViewBag.LoginMessage = "NotAuthenticated";
                return View(objLogIn);
            }
        }
            catch (Exception ex)
            {
                ViewBag.LoginMessage = "TroubleInLogin";
                return View(objLogIn);
    }
}

        private bool TbUserActivityUserIdExists(int UserId)
{
    return _appcontext.TbUserActivity.Any(e => e.UserId == UserId);
}
private AllUserViewModel Validate(string userName, string password)
{
    var isValid = ValidateCredentials(userName, password);
    var userDetail = new AllUserViewModel();
    if (isValid)
    {
        string userencryptedpws = RandomPasswordGenerator.encode(password.ToString().Trim());
        var portalUserList = (from portalUser in _appcontext.TbUserMasters
                              where portalUser.Status == true
                              select new AllUserViewModel()
                              {
                                  UserId = portalUser.UserId,
                                  UserFullName = portalUser.FirstName + " " + (portalUser.LastName == null ? "" : portalUser.LastName),
                                  UserName = portalUser.UserName,
                                  Password = portalUser.Password,
                                  UserTypeId = portalUser.UserTypeId,
                                  Email = portalUser.Email
                              }).ToList();
        List<AllUserViewModel> auvmList = portalUserList;

        userDetail = (from t1 in auvmList
                      where (t1.UserName.ToLower() == userName.ToLower() && t1.Password.Trim() == userencryptedpws)
                      || (t1.CallSignNo == userName.ToLower() && t1.Password.Trim() == userencryptedpws)
                      select new AllUserViewModel
                      {
                          UserId = t1.UserId,
                          UserFullName = t1.UserFullName,
                          UserName = t1.UserName,
                          Password = t1.Password,
                          UserTypeId = t1.UserTypeId,
                          Email = t1.Email
                      }).FirstOrDefault();
        return userDetail;
    }

    //return userDetail;
    return null;
}

private bool ValidateCredentials(string userName, string password)
{
    string userencryptedpws = RandomPasswordGenerator.encode(password.ToString().Trim());

    var portalUserList = (from portalUser in _appcontext.TbUserMasters
                          where portalUser.Status == true
                          select new AllUserViewModel()
                          {
                              UserId = portalUser.UserId,
                              UserFullName = portalUser.FirstName + " " + (portalUser.LastName == null ? "" : portalUser.LastName),
                              UserName = portalUser.UserName,
                              Password = portalUser.Password,
                              UserTypeId = portalUser.UserTypeId
                          }).ToList();
    List<AllUserViewModel> auvmList = portalUserList;

    var userDetail = (from t1 in auvmList
                      where (t1.UserName.ToLower() == userName.ToLower() && t1.Password.Trim() == userencryptedpws)
                      || (t1.CallSignNo == userName.ToLower() && t1.Password.Trim() == userencryptedpws)
                      select new AllUserViewModel
                      {
                          UserId = t1.UserId,
                          UserFullName = t1.UserFullName,
                          UserName = t1.UserName,
                          Password = t1.Password,
                          CallSignNo = t1.CallSignNo,
                          UserTypeId = t1.UserTypeId
                      }).FirstOrDefault();

    if (userDetail != null)
    {
        return true;
    }
    else
    {
        return false;
    }
}

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public IActionResult Error()
{
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
public ActionResult GetMenuList()
{
    try
    {
        var result = (from m in _appcontext.TbMenuMasters
                      select new MenuMasterEntity
                      {
                          MenuId = m.MenuId,
                          Name = m.Name,
                          Description = m.Description,
                          Sequence = m.Sequence,
                          Title = m.Title,
                          Icon = m.Icon,
                          Url = m.Url,
                          ParentMenuId = m.ParentMenuId,
                          HasSubMenu = m.HasSubMenu,
                          Status = m.Status,

                      }).ToList();
        return PartialView("SideBarNav", result);
    }
    catch (Exception ex)
    {
        var error = ex.Message.ToString();
        return Content("Error");
    }
}

public ActionResult ChangePassword()
{
    int principalId = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
    if (principalId == 0)
    {
        return RedirectToAction("Login", "Home");
    }
    ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel();
    changePasswordViewModel = (from portalUserMaster in _appcontext.TbUserMasters
                               where portalUserMaster.UserId == principalId && portalUserMaster.Status == true
                               select new ChangePasswordViewModel()
                               {
                                   UserId = portalUserMaster.UserId,
                                   UserName = portalUserMaster.UserName,
                                   Password = string.Empty,
                                   NewPassword = string.Empty,
                                   ConfirmPassword = string.Empty,
                               }).FirstOrDefault();
    return View(changePasswordViewModel);
    //HttpContext.Session.Clear();
    //return RedirectToAction("Login", "Home");
}

[HttpPost]
public ActionResult ChangePassword(ChangePasswordViewModel objChangePassword)
{
    try
    {
        if (ModelState.IsValid)
        {
            var userData = Validate(HttpContext.Session.GetString(Common.UserId), objChangePassword.Password);
            if (userData != null)
            {
                int principalId = HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
                TbUserMaster tbPortalUserMaster = new TbUserMaster();
                tbPortalUserMaster = (from portalUserMaster in _appcontext.TbUserMasters
                                      where portalUserMaster.UserId == principalId && portalUserMaster.Status == true
                                      select portalUserMaster).FirstOrDefault();
                tbPortalUserMaster.Password = RandomPasswordGenerator.encode(objChangePassword.NewPassword.ToString().Trim());
                tbPortalUserMaster.LastUpdatedOn = DateTime.Now;
                tbPortalUserMaster.LastUpdatedBy = principalId;
                tbPortalUserMaster.Status = true;
                _appcontext.Update(tbPortalUserMaster);
                _appcontext.SaveChanges();
                ViewBag.LoginMessage = "PasswordChanged";
                return View(objChangePassword);
            }
            else
            {
                ViewBag.LoginMessage = "NotAuthenticated";
                return View(objChangePassword);
            }
        }
        else
        {
            //ViewBag.LoginMessage = "NotAuthenticated";
            return View(objChangePassword);
        }
    }
    catch (Exception ex)
    {
        //ViewBag.LoginMessage = "TroubleInLogin";
        return View(objChangePassword);
    }
}

public ActionResult GoToHome()
{
    int groupId = HttpContext.Session.GetInt32(Common.GroupId) ?? 0;
    if (groupId == (Int32)UserGroupEnum.SuperUser || groupId == (Int32)UserGroupEnum.Admin)
    {
        return RedirectToAction("Index", "Run");
    }
    return RedirectToAction("Index", "Run");
}

public ActionResult ForgotPassword()
{
    return View();
}

[HttpPost]
public ActionResult ForgotPassword(ForgotPasswordViewModel objForgot)
{
    try
    {
        if (ModelState.IsValid)
        {
            var tbPortalUserMaster = (from portalUser in _appcontext.TbUserMasters
                                      where portalUser.Email == objForgot.Email && portalUser.Status == true
                                      select portalUser).FirstOrDefault();

            if (tbPortalUserMaster != null)
            {
                string newPass = RandomPasswordGenerator.generatePassword(6, true);
                tbPortalUserMaster.Password = RandomPasswordGenerator.encode(newPass.Trim());
                tbPortalUserMaster.LastUpdatedOn = DateTime.Now;
                tbPortalUserMaster.LastUpdatedBy = tbPortalUserMaster.UserId;
                tbPortalUserMaster.Status = true;
                _appcontext.Update(tbPortalUserMaster);
                _appcontext.SaveChanges();

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

                bool result = SendEmail("eSanjeevani Portal Credential", htmlString, tbPortalUserMaster.Email);
                ViewBag.PasswordSuccess = "MailSent";
            }
        }
        else
        {
            return View(objForgot);
        }
    }
    catch (Exception ex)
    {
        var innerexc = ex.InnerException.Message.ToString();
        return View(objForgot);
    }
    ViewBag.PasswordSuccess = "PasswordSuccess";
    return RedirectToAction("ForgotPasswordMessage", "Home");
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
public IActionResult ForgotPasswordMessage()
{
    return View();
}
    }
}
