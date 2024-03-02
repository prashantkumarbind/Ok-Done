using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eSanjeevaniIcu.Shared.Enum
{
    public enum UserGroupEnum
    {
        SuperUser = 1,
        Admin = 2,
        Doctor = 3,
        Nurse = 4,
        Specialist = 5,
        Other

    }
    public enum JVCategoryEnum
    {
        Permanent = 1,
        Casual = 2
    }
    public enum JVJobTypeEnum
    {
        Fulltime = 1,
        Parttime = 2
    }
    public enum MBTypeEnum
    {
        //[DisplayName("Emergency / Critical")]
        Critical = 1,

        Important = 2,

        //[DisplayName("General")]
        General = 3,

        //[DisplayName("Shift Notification")]
        Shift = 4
    }
    public enum UserActionEnum
    {
        //[DisplayName("Accepted")]
        Accepted = 0,

        //[DisplayName("Rejected")]
        Rejected = 1,

        //[DisplayName("Not Responded")]
        NotResponded = 2,

        //[DisplayName("Assigned")]
        Assigned = 3,

        //[DisplayName("Not Assigned")]
        UnAssigned = 4
    }
    public enum MBLocationEnum
    {
        All = 0,
        Geographical = 1,
        Client = 2,
        Employee = 3,
        //Commented By Ravi On Dated 31-March-2020
        //[DisplayName("Geo Location")]
        //GeoLocation = 3
    }
    public enum UserStatus
    {
        Deactive = 0,
        Activate = 1,
        PendingForApproval = 3
    }
    public enum LeaveApplicationStatus
    {
        [Display(Name = "Pending For Approval")]
        PendingForApproval = 0,
        [Display(Name = "Approved")]
        Approved = 1,
        [Display(Name = "Rejected")]
        Rejected = 2,
        [Display(Name = "Pending For Cancellation")]
        PendingForCancelation = 3,
        [Display(Name = "Cancelled")]
        Cancelled = 4

    }

    public enum LeaveType
    {
        [Display(Name = "Long Service Leave")]
        LongServiceLeave = 0,

        [Display(Name = "Annual Leave")]
        AnnualLeave = 1,

        [Display(Name = "Personal Leave")]
        PersonalLeave = 2,

        [Display(Name = "Sick Leave")]
        SickLeave = 3,

        [Display(Name = "Leave Without Pay")]
        LeaveWithoutPay = 4,
    }

    public enum UserRequestActionEnum
    {

        Open = 0,

        Closed = 1,

        //Rejected = 2,
    }

    public enum LeaveStatusEnum
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Cancelled = 4,
        CancellationPending = 5,
        PendingModification = 6,
        RejectCancellation = 7,
    }
    public enum PatientStatus
    {
        Normal = 1,
        High = 2,
        Critical = 3
    }

    public enum GenderName
    {
        Male = 1,
        Female = 2
    }
}
