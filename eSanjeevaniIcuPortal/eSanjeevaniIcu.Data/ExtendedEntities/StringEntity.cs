using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eSanjeevaniIcu.Data.ExtendedEntities
{
    public class StringEntity
    {
        [Key]
        public string StringResult { get; set; }
    }
}
