using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace eSanjeevaniIcu.Shared.Resource
{
    public enum AdminBusinessRuleConstant
    {
        [Description("Invalid Username or password")]
        InvalidLoginCredentials = 10000,

        [Description("Your account is Locked or Disabled. Please contact Administrator")]
        UserAccountInActiveLocked = 10001,

        [Description("You do not have access to the requested function.")]
        PermissionDenied = 10002
    }
}
