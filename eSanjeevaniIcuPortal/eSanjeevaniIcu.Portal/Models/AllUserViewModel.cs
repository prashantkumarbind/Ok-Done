using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class AllUserViewModel
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CallSignNo { get; set; }
        public int UserTypeId { get; set; }

        public List<AllUserViewModel> auvmList { get; set; }
    }
}
