using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbUserDetail
{
    public int UserDetailId { get; set; }

    public int UserId { get; set; }

    public string SurName { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string Image { get; set; }

    public string Signature { get; set; }

    public int Gender { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string RegistrationNumber { get; set; }

    public int? QualificationId { get; set; }

    public int? Experience { get; set; }

    public int CoeId { get; set; }

    public int SpecialityId { get; set; }

    public int SpokeHospitalId { get; set; }

    public string Language { get; set; }

    public string FacebookProfile { get; set; }

    public string LinkedinProfile { get; set; }

    public string TwitterProfile { get; set; }

    public string PlaceOfWork { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public int StateId { get; set; }

    public int DistrictId { get; set; }

    public int CityId { get; set; }

    public string PinCode { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }
}
