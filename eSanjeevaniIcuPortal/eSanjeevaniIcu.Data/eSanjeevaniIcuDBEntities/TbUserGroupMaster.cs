using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbUserGroupMaster
{
    public int UserGroupId { get; set; }

    public string UserGroupName { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<TbGroupPermissions> TbGroupPermissions { get; set; } = new List<TbGroupPermissions>();

    public virtual ICollection<TbUserMaster> TbUserMasters { get; set; } = new List<TbUserMaster>();
}
