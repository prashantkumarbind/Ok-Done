using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbPatientMaster
{
    public int PatientId { get; set; }

    public int SpokeHospitalId { get; set; }

    public string PatientName { get; set; }

    public string PatientCode { get; set; }

    public string Description { get; set; }

    public string ContactNo { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public int? Pin { get; set; }

    public string BedNo { get; set; }

    public DateTime AdmissionDate { get; set; }

    public int? Gender { get; set; }

    public int? Age { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }
}
