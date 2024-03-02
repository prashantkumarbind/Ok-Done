using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Controllers;
using eSanjeevaniIcu.Portal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace eSanjeevaniIcu.Portal.Component
{
    public class SideBarNav:ViewComponent
    {
        private readonly eSanjeevaniIcuDbContext _context;

        private readonly CommonFunctions _commonFunctions;
        private ApplicationConfigurations ApplicationConfigurations { get; set; }
        public SideBarNav(eSanjeevaniIcuDbContext context, IOptions<ApplicationConfigurations> settings)
        {
            _context = context;
            _commonFunctions = new CommonFunctions(_context);
            ApplicationConfigurations = settings.Value;
        }
        public IViewComponentResult Invoke()
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
                                                         Url = lstMenuMaster.Url.StartsWith("/") == true ? ApplicationConfigurations.ApplicationHostUrl + "" + lstMenuMaster.Url : lstMenuMaster.Url,
                                                         ParentMenuId = lstMenuMaster.ParentMenuId,
                                                         ParentMenuName = lstMenuMaster.ParentMenuName,
                                                         HasSubMenu = lstMenuMaster.HasSubMenu,
                                                         Status = lstMenuMaster.Status
                                                     }).ToList();
                return View(menuMasterViewModel.lstMenuMaster);
            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
                return Content("Error");
            }
        }
    }
}
