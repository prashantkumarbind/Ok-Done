using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class GroupMasterViewModel
    {
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Group Name is Required.")]
        public string GroupName { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public List<GroupMasterViewModel> lstGroupMaster { get; set; }

        public bool IsDeletable { get; set; }
    }
}
