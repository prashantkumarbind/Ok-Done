using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbBedMaster
{
    public int BedId { get; set; }

    public int IcuId { get; set; }

    public int SpokeHospitalId { get; set; }

    public string BedType { get; set; }

    public string BedNumber { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }
}
