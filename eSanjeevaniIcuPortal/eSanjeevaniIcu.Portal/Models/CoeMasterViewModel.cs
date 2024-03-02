using System;

namespace eSanjeevaniIcu.Portal.Models
{
    public class CoeMasterViewModel
    {
        public int CoeId { get; set; }

        public string CoeCode { get; set; }

        public string CoeName { get; set; }
        public string Image { get; set; }
        public string ContactNumber { get; set; }

        public string EmailAddress { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public int DistrictId { get; set; }

        public int CityId { get; set; }

        public int? StateId { get; set; }

        public string PinCode { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool Status { get; set; }
    }
}
