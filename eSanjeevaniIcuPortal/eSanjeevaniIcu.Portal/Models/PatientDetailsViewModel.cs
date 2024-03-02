using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class PatientDetailsViewModel
    {
        public int PatientDetailId { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; }

        public DateTime Date { get; set; }

        public decimal? Temperature { get; set; }

        public decimal? RrRespiratoryRate { get; set; }

        public decimal? OxygenSaturationSpo2 { get; set; }

        public decimal? BloodPressureDia { get; set; }

        public decimal? BloodPressureSys { get; set; }

        public decimal? HeartRate { get; set; }

        public int? PatientStatus { get; set; }
        public string PatientStatusName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public int? LastUpdatedBy { get; set; }

        public bool Status { get; set; }
        public string StatusName { get; set; }
        public List<SelectListItem> lstPatientMaster { get; set; }
        public List<SelectListItem> lstPatientStatus { get; set; }
        public List<PatientDetailsViewModel> lstPatientDetailsViewModel { get; set; }
    }
}
