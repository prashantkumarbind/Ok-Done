using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbHubHospital
{
    public int HubHospitalId { get; set; }

    public string HubHospitalName { get; set; }

    public string ContactNo { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public int? Pin { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }
}
