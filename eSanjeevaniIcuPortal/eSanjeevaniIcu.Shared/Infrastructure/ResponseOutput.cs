using eSanjeevaniIcu.Shared.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace eSanjeevaniIcu.Shared.Infrastructure
{
    public class ResponseOutput<T> : IResponseOutput<T>
    {
        public bool IsSuccessful { get; set; }

        public T Result { get; set; }

        public IEnumerable<RuleOutcome> RuleOutcome { get; set; }
    }
}
