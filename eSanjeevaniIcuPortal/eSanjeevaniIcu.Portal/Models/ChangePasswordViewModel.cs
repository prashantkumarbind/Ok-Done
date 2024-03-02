using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
