using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class IcuViewModel
    {
        public int IcuId { get; set; }
        public string IcuName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int SpokeHospitalId { get; set; }
        public string SpokeHospitalName { get; set; }
        public int TotalBeds { get; set; }
        public int HduBeds { get; set; }
        public int IcuBeds { get; set; }
        public string Other { get; set; }
        public int? Beds { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public List<IcuViewModel> lstIcuViewModel { get; set; }
        public List<SelectListItem> lstSpokeHospital { get; set; }
        public List<SelectListItem> lstStateMaster { get; set; }
    }
}
