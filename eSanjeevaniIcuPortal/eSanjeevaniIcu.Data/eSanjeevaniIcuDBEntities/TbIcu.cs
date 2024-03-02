using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbIcu
{
    public int IcuId { get; set; }

    public string IcuName { get; set; }

    public int StateId { get; set; }

    public int SpokeHospitalId { get; set; }

    public int TotalBeds { get; set; }

    public int HduBeds { get; set; }

    public int IcuBeds { get; set; }

    public string Other { get; set; }

    public int? Beds { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }
}
