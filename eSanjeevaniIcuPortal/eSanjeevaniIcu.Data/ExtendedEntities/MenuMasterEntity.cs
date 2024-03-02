using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eSanjeevaniIcu.Data.ExtendedEntities
{
    public class MenuMasterEntity
    {
        [Key]
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Sequence { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int? ParentMenuId { get; set; }
        public string ParentMenuName { get; set; }
        public bool? HasSubMenu { get; set; }
        public bool Status { get; set; }
        
    }
}
