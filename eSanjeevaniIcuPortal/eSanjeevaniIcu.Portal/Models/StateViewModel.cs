using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eSanjeevaniIcu.Portal.Models
{
    public class StateViewModel
    {
        public int StateId { get; set; }

        public string StateCode { get; set; }

        [Required(ErrorMessage = "State Name is Required.")]
        public string StateName { get; set; }

        public int CountCoe { get; set; }

        public bool HasStateAdmin { get; set; }
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public int? LastUpdatedBy { get; set; }

        public bool Status { get; set; }
        public List<StateViewModel> lstStateMaster { get; set; }
        public List<SelectListItem> lstStateMasterMain { get; set; }
        //public IEnumerable<SelectListItem> lstStateMasterMain { get; set; }


    }
}
