using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbCoeMaster
{
    public int CoeId { get; set; }

    public string CoeCode { get; set; }

    public string CoeName { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }

    public string Honor { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public string EmailAddress { get; set; }

    public string ContactNumber { get; set; }

    public int CityId { get; set; }

    public int StateId { get; set; }

    public int DistrictId { get; set; }

    public string PinCode { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string Designation { get; set; }

    public string PlaceOfWork { get; set; }

    public string ProfileImage { get; set; }

    public bool IsDeleted { get; set; }
}
