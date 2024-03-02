using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbIndianState
{
    public int StateId { get; set; }

    public string StateCode { get; set; }

    public string StateName { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }
}
