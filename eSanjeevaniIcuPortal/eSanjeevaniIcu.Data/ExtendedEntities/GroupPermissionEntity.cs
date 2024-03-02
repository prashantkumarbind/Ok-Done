using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace eSanjeevaniIcu.Data.ExtendedEntities
{
    public class GroupPermissionEntity
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string MenuIds { get; set; }
        public string MenuNames { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
    }
}
