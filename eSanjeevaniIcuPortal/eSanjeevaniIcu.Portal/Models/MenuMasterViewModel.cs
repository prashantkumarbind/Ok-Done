using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class MenuMasterViewModel
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Sequence { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int? ParentMenuId { get; set; }
        public string ParentMenuName { get; set; }
        public bool? HasSubMenu { get; set; }
        public bool Status { get; set; }
        public int? DataOfDays { get; set; }
        public List<MenuMasterViewModel> lstMenuMaster { get; set; }
        public List<SelectListItem> lstParentMenus { get; set; }
    }
}
