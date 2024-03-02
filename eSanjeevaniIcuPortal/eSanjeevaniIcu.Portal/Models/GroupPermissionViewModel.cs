using eSanjeevaniIcu.Data.ExtendedEntities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class GroupPermissionViewModel
    {      

        public int GroupPermissionId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuIds { get; set; }
        public string MenuNames { get; set; }
        public int[] selectedMenuIds { get; set; }
        public bool? Status { get; set; }
        public string StatusName { get; set; }
        public List<SelectListItem> lstMenus { get; set; }
        public List<SelectListItem> lstGroup { get; set; }
        public List<GroupPermissionViewModel> lstGroupPermissions { get; set; }

        public List<MenusWithPermissionEntity> lstMenusWithPermissionEntity { get; set; }

    }
}
