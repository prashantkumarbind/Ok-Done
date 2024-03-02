using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;

namespace eSanjeevaniIcu.Portal.Models
{
    public class PatientViewModel
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }        
        public string ContactNumber { get; set; }        
        public string EmailAddress { get; set; }
        public int Weight { get; set; }
        public string BloodGroup { get; set; }
        public DateTime AdmitDate { get; set; }
        public int IcuId { get; set; }
        public string IcuName { get; set; }
        public int? BedId { get; set; }
        public string BedName { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Address { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }        
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string PinCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int LastUpdatedBy { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public string Serverity { get; set; }
        //public int Age { get; set; }
        //public int SpokeHospitalId { get; set; }
        public List<PatientViewModel> lstPatientViewModel { get; set; }
        public List<SelectListItem> lstICU { get; set; }
        public List<SelectListItem> lstBed { get; set; }
        public List<SelectListItem> lstDoctor { get; set; }
        public List<SelectListItem> lstStateMaster { get; set; }
        public List<SelectListItem> lstCityMaster { get; set; }
        public List<SelectListItem> lstDistrictMaster { get; set; }
    }
}
