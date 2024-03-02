using System;
using System.Collections.Generic;
using System.Text;

namespace eSanjeevaniIcu.Shared.Infrastructure.Contract
{
    public interface IResponseOutput<T>
    {
        T Result { get; set; }

        IEnumerable<RuleOutcome> RuleOutcome { get; set; }

        bool IsSuccessful { get; set; }
    }
}
