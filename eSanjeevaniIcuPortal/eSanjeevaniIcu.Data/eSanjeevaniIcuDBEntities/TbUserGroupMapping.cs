using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbUserGroupMapping
{
    public int UserGroupMappingId { get; set; }

    public int PrincipalId { get; set; }

    public int GroupId { get; set; }

    public bool? Status { get; set; }
}
