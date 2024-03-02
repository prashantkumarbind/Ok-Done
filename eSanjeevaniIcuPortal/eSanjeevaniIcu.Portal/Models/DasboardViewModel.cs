using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class DasboardViewModel
    {
        public int PatientDetailId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientCode { get; set; }
        public string BedNo { get; set; }
        public string IcuNo { get; set; }
        public DateTime Date { get; set; }
        public decimal? Temperature { get; set; }

        public decimal? RrRespiratoryRate { get; set; }

        public decimal? OxygenSaturationSpo2 { get; set; }

        public decimal? BloodPressureDia { get; set; }

        public decimal? BloodPressureSys { get; set; }

        public decimal? HeartRate { get; set; }

        public int? PatientStatus { get; set; }
        public string PatientStatusName { get; set; }

        public int? Gender { get; set; }

        public int? Age { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime AdmissionDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public int? LastUpdatedBy { get; set; }

        public int? HubHospitalId { get; set; }
        public List<SelectListItem> lstHubHopital { get; set; }
        public int? SpokeHospitalId { get; set; }
        public string SpokeHospitalName { get; set; }
        public List<SelectListItem> lstSpokeHopital { get; set; }
        public List<GraphViewModel> lstGraphViewModel { get; set; }
        public string GenderName { get; set; }
        public string LoggedInUserName { get; set; }
    }
    public class GraphViewModel
    {
        public DateTime Date { get; set; }
        public decimal? Temperature { get; set; }

        public decimal? RrRespiratoryRate { get; set; }

        public decimal? OxygenSaturationSpo2 { get; set; }

        public decimal? BloodPressureDia { get; set; }

        public decimal? BloodPressureSys { get; set; }

        public decimal? HeartRate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
