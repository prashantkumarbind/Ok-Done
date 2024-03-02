using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Portal.Models.CoE
{
    public class CoeModel
    {
        public int CoeId { get; set; }

        public string CoeCode { get; set; }

        public string CoeName { get; set; }
        public string ProfileImage { get; set; }
        public string ContactNumber { get; set; }

        public string EmailAddress { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }
        public string Honor { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string PlaceOfWork { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string PinCode { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public List<SelectListItem> lstStateMaster { get; set; }
        public List<SelectListItem> lstCityMaster { get; set; }
        public List<SelectListItem> lstDistrictMaster { get; set; }
    }
}
