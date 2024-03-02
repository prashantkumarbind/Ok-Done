using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbStateAdmin
{
    public int StateAdminId { get; set; }

    public int StateId { get; set; }

    public string SurName { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string Designation { get; set; }

    public string PlaceOfWork { get; set; }

    public string Address { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int? LastUpdatedBy { get; set; }

    public bool Status { get; set; }
}
