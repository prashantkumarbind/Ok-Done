using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eSanjeevaniIcu.Data.ExtendedEntities
{
    public class StatusVsUserCountEntity
    {
        [Key]
        public int ActivationStatus { get; set; }
        public string ActivationStatusName { get; set; }
        public int UserCount { get; set; }
    }
}
