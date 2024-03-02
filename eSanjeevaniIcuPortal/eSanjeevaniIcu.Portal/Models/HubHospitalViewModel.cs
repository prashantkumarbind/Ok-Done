using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class HubHospitalViewModel
    {
        public int HubHospitalId { get; set; }

        public string HubHospitalName { get; set; }

        public string ContactNo { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int? Pin { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public int? LastUpdatedBy { get; set; }

        public bool Status { get; set; }
        public string StatusName { get; set; }
        public List<HubHospitalViewModel> lstHubHospitalViewModel { get; set; }
    }
}
