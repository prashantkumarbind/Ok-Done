using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Controllers;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace eSanjeevaniIcu.Portal.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly CommonController _commonController;
        private readonly eSanjeevaniIcuDbContext _appcontext;
        private readonly CommonFunctions _commonFunctions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private EmailConfiguration EmailConfigurations { get; set; }
        private ApplicationConfigurations ApplicationConfigurations { get; set; }
        
        public NotificationHub(eSanjeevaniIcuDbContext rcAppDbContext, IOptions<ApplicationConfigurations> settings, IOptions<EmailConfiguration> emailSettings, IHttpContextAccessor httpContextAccessor)
        {
            _appcontext = rcAppDbContext;
            _commonFunctions = new CommonFunctions(_appcontext);
            _commonController = new CommonController(_appcontext, settings, emailSettings);
            EmailConfigurations = emailSettings.Value;
            ApplicationConfigurations = settings.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public override Task OnConnectedAsync()
        {
            InsertSignalRCon(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            DeleteSignalRCon(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public string InsertSignalRCon(string ConnectionId)
        {
            try
            {
                TbSignalRcon sr = new TbSignalRcon();
                sr.UserId = (int) _httpContextAccessor.HttpContext.Session.GetInt32(Common.PrincipalId);
                sr.ConnectionId = ConnectionId;
                sr.Status = true;
                _appcontext.Add(sr);
                _appcontext.SaveChanges();
                return "Ok";
            }
            catch (Exception ex)
            {
                return "NotOk";
            }
        }
       
        public string DeleteSignalRCon(string ConnectionId)
        {
            try
            {
                TbSignalRcon sr = new TbSignalRcon();
                sr = (from t1 in _appcontext.TbSignalRcons
                      where t1.UserId == (int) _httpContextAccessor.HttpContext.Session.GetInt32(Common.PrincipalId) && t1.ConnectionId == ConnectionId
                      select new TbSignalRcon
                      {
                          Srcid = t1.Srcid,
                          UserId = t1.UserId,
                          ConnectionId = t1.ConnectionId,
                          Status = t1.Status,
                          CreatedOn = t1.CreatedOn,
                      }).FirstOrDefault();
                sr.Status = false;
                _appcontext.Update(sr);
                _appcontext.SaveChanges();
                return "Ok";
            }
            catch (Exception ex)
            {
                return "NotOk";
            }
        }
    
    }

}
