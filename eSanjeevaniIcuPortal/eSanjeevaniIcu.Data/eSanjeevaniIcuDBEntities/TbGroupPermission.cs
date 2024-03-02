using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbGroupPermission
{
    public int GroupPermissionId { get; set; }

    public int GroupId { get; set; }

    /// <summary>
    /// It will contains comma separated Menu Ids
    /// </summary>
    public int MenuId { get; set; }

    public bool Status { get; set; }
}
