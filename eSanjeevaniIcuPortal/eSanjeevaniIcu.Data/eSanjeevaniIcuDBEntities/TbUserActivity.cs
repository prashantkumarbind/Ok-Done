using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbUserActivity
{
    public int UserActivityId { get; set; }

    public int UserId { get; set; }

    public int UserTypeId { get; set; }

    public DateTime LoginTime { get; set; }

    public DateTime? LogoutTime { get; set; }
    public bool IsLogin { get; set; }

    public bool? Status { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
