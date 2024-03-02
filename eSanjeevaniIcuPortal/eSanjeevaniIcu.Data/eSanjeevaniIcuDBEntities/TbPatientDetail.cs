using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbPatientDetail
{
    public int PatientDetailId { get; set; }

    public int PatientId { get; set; }

    public DateTime Date { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? RrRespiratoryRate { get; set; }

    public decimal? OxygenSaturationSpo2 { get; set; }

    public decimal? BloodPressureDia { get; set; }

    public decimal? BloodPressureSys { get; set; }

    public decimal? HeartRate { get; set; }

    public int? PatientStatus { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }
}
