using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eSanjeevaniIcu.Data.ExtendedEntities
{
    public class UserCountEntity
    {
        [Key]
        public int Activated { get; set; }
        public int Deactivated { get; set; }
        public int Pending { get; set; }

        public int Total { get; set; }
    }
}
