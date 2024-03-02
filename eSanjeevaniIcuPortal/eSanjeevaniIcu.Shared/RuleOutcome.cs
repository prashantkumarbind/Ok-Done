using eSanjeevaniIcu.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace eSanjeevaniIcu.Shared
{
    public class RuleOutcome
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public RuleOutcomeStatus Status { get; set; }
    }
}
