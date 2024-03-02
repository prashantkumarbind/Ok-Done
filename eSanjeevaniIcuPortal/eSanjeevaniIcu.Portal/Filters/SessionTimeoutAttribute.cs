using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eSanjeevaniIcu.Portal.Filters
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int sessPrincipalId = filterContext.HttpContext.Session.GetInt32(Common.PrincipalId) ?? 0;
            if (sessPrincipalId == 0)
            {
                filterContext.Result = new RedirectResult("~/Home/LogIn");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}