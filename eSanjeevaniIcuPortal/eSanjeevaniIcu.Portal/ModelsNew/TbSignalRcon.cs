using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Portal.ModelsNew;

public partial class TbSignalRcon
{
    public int Srcid { get; set; }

    public int UserId { get; set; }

    public string ConnectionId { get; set; }

    public bool Status { get; set; }

    public DateTime? CreatedOn { get; set; }
}
