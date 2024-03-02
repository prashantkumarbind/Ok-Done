using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbGroupMaster
{
    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public bool Status { get; set; }
}
