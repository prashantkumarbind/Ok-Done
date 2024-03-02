using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Portal.Models.CoE
{
    public class MappingViewModel
    {
        public List<MappingModel> Mappings { get; set; }
        public List<SelectListItem> lstCoEMaster { get; set; }
        public List<SelectListItem> lstSpokeMaster { get; set; }
        public List<SelectListItem> lstSpecialistMaster { get; set; }
    }
}
