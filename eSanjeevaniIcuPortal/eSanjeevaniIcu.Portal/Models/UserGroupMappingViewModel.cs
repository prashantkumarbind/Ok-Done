using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class UserGroupMappingViewModel
    {
        public int UserGroupMappingId { get; set; }
        public int PrincipalId { get; set; }
        public string UserName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool? Status { get; set; }
        public string StatusName { get; set; }
        public List<SelectListItem> lstUser { get; set; }
        public List<SelectListItem> lstGroup { get; set; }
        public List<UserGroupMappingViewModel> lstUserGroupMappings { get; set; }
    }
}
