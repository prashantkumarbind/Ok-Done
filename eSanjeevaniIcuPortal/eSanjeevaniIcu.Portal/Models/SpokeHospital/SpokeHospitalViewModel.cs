using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models.SpokeHospital
{
    public class SpokeHospitalViewModel
    {
        public int SpokeHospitalId { get; set; }

        public int HubHospitalId { get; set; }

        public string HubHospitalName { get; set; }
        public string SpokeHospitalName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int District { get; set; }
        public string DistrictName { get; set; }
        public int City { get; set; }
        public string CityName { get; set; }
        public int State { get; set; }
        public string StateName { get; set; }
        public int? Pin { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public List<SpokeHospitalViewModel> lstSpokeHospitalViewModel { get; set; }
        public List<SelectListItem> lstHubHospital { get; set; }
        public List<SelectListItem> lstCityMaster { get; set; }
        public List<SelectListItem> lstDistrictMaster { get; set; }
        public List<SelectListItem> lstStateMaster { get; set; }
    }
}
