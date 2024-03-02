using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities
{
    public partial class TbGroupPermissions
    {
        public int GroupPermissionId { get; set; }
        public int GroupId { get; set; }
        public int MenuId { get; set; }
        public bool Status { get; set; }
    }
}
