using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class PortalUserViewModel
    {
        public int UserId { get; set; }
        public string EmployeeCode { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; internal set; }
        public List<PortalUserViewModel> lstPortalUserViewModel { get; set; }
    }
}
