using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Portal.Models.SpokeHospital
{
    public class MappingViewModel
    {
        public List<MappingModel> Mappings { get; set; }
        public List<SelectListItem> lstSpokehospital { get; set; }
        public List<SelectListItem> lstDoctors { get; set; }
        public List<SelectListItem> lstNurses { get; set; }
    }
}
