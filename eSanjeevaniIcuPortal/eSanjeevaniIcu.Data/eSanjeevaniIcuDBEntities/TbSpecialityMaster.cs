using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbSpecialityMaster
{
    public int SpecialityId { get; set; }

    public string SpecialityName { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastupdatedBy { get; set; }

    public bool Status { get; set; }
}
