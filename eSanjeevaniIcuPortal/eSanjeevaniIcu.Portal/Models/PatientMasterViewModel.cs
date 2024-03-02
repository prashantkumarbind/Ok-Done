using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class PatientMasterViewModel
    {
        public int PatientId { get; set; }

        public int SpokeHospitalId { get; set; }
        public string SpokeHospitalName { get; set; }

        public string PatientName { get; set; }
        public string PatientCode { get; set; }

        public string Description { get; set; }

        public string ContactNo { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int? Pin { get; set; }

        public string BedNo { get; set; }

        public DateTime AdmissionDate { get; set; }

        public int? Gender { get; set; }

        public int? Age { get; set; }
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public int? LastUpdatedBy { get; set; }

        public bool Status { get; set; }
        public string StatusName { get; set; }
        public List<PatientMasterViewModel> lstPatientMasterViewModel { get; set; }
        public List<SelectListItem> lstSpokeHospital { get; set; }
    }
}
