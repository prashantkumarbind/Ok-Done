using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eSanjeevaniIcu.Portal.Models
{
    public class PatientCommunicationViewModel
    {
        public int PatientCommunicationId { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public int? LastUpdatedBy { get; set; }

        public bool Status { get; set; }
        public bool CurrentUser { get; set; }

        public List<PatientCommunicationViewModel> lstPatientCommunicationViewModel { get; set; }
    }
}
