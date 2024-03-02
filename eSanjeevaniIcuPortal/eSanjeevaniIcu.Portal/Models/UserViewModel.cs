using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "First Name is Required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Mobile Number is Required.")]
        //[RegularExpression(@"^[0-9]\d{9,10}$|^[1-9]\d{8,9}$", ErrorMessage = "Enter valid Mobile Number.")]
        public string MobileNumber { get; set; }
        
        [Required(ErrorMessage = "Email Address is Required.")]
        [RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$", ErrorMessage = "Enter valid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User Name is Required.")]
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Select User Type.")]
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public bool? Status { get; set; }        

        public List<SelectListItem> lstUserType { get; set; }
        
        public string StatusName { get; internal set; }

        public int GroupId { get; set; }
        public int StatusId { get; set; }
        public List<UserViewModel> lstUserViewModel { get; set; }
    }
}
