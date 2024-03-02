using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbSpokeHospital
{
    public int SpokeHospitalId { get; set; }

    public int HubHospitalId { get; set; }

    public string SpokeHospitalName { get; set; }

    public string ContactNo { get; set; }

    public string Email { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public int City { get; set; }

    public int District { get; set; }

    public int State { get; set; }

    public int? Pin { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }
}
