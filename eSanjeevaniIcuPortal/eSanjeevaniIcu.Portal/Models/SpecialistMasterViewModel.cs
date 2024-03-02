using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Models
{
    public class SpecialistMasterViewModel
    {
        public int SpecialistId { get; set; }
        public string SurName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int Gender { get; set; }
        public string Image { get; set; }
        public string Signature { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string RegistrationNumber { get; set; }
        public int? QualificationId { get; set; }
        public string QualificationName { get; set; }
        public int CoeId { get; set; }
        public string CoeName { get; set; }
        public int SpecialityId { get; set; }
        public string SpecialityName { get; set; }
        public int? Experience { get; set; }
        public string Language { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string PinCode { get; set; }
        public string PlaceOfWork { get; set; }
        public string FacebookProfile { get; set; }
        public string LinkedinProfile { get; set; }
        public string TwitterProfile { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public List<SpecialistMasterViewModel> lstSpecialistMasterViewModel { get; set; }
        public List<SelectListItem> lstQualificationMaster { get; set; }
        public List<SelectListItem> lstSpecialityMaster { get; set; }
        public List<SelectListItem> lstCoeMaster { get; set; }
        public List<SelectListItem> lstStateMaster { get; set; }
        public List<SelectListItem> lstCityMaster { get; set; }
        public List<SelectListItem> lstDistrictMaster { get; set; }
    }
}
