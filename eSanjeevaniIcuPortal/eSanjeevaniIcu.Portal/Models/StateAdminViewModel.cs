using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eSanjeevaniIcu.Portal.Models
{
    public class StateAdminViewModel
    {
       public int StateAdminId { get; set; }

        public int StateId { get; set; }

        public string StateName { get; set; }

        public string SurName { get; set; }

        [Required(ErrorMessage = "First Name is Required.")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone Number is Required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is Required.")]
        public string Email { get; set; }

        public string Designation { get; set; }

        //[Required(ErrorMessage = "Place Of Work is Required.")]
        public string PlaceOfWork { get; set; }

        public string Address { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public int? LastUpdatedBy { get; set; }

        public bool Status { get; set; }

        public List<SelectListItem> lstStateAdmin { get; set; }


    }
}
