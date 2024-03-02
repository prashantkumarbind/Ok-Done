using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class PTable
{
    public int Id { get; set; }

    public int? PId { get; set; }

    public string PLastName { get; set; }

    public string PFirstName { get; set; }

    public int? PSys { get; set; }

    public int? PDia { get; set; }

    public int? PMean { get; set; }

    public int? PPulseRate { get; set; }

    public int? POxim { get; set; }

    public string TimeStamp { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
}
