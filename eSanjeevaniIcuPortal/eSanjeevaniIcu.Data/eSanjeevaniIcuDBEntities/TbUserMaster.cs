using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbUserMaster
{
    public int UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MobileNumber { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }

    public int UserTypeId { get; set; }
}
