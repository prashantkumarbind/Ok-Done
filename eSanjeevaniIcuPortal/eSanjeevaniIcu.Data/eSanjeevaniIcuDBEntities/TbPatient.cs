using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbPatient
{
    public int PatientId { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }    

    public int Gender { get; set; }

    public string ContactNumber { get; set; }

    public string EmailAddress { get; set; }

    public int Weight { get; set; }

    public string BloodGroup { get; set; }

    public DateTime AdmitDate { get; set; }

    public int IcuId { get; set; }

    public int? BedId { get; set; }

    public int DoctorId { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public int DistrictId { get; set; }

    public int CityId { get; set; }

    public int StateId { get; set; }

    public string PinCode { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }

    public string Serverity { get; set; }

    //public int Age { get; set; }
    //public int SpokeHospitalId { get; set; }
}
