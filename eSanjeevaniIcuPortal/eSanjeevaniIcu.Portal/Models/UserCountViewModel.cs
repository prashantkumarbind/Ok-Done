using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class UserCountViewModel
    {
        public int Active { get; set; }
        public int Inactive { get; set; }
        [Column("Pending For Approval")]
        public int PendingForApproval { get; set; }
        public int Rejected { get; set; }
        public int Total { get; set; }

        public List<UserCountViewModel> lstUserCountViewModel { get; set; }
        public List<StatusVsUserCountViewModel> lstStatusVsUserCountViewModel { get; set; }
    }
}
