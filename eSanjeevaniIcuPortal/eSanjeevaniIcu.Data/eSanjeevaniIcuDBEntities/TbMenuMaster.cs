using System;
using System.Collections.Generic;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class TbMenuMaster
{
    public int MenuId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal? Sequence { get; set; }

    public string Title { get; set; }

    public string Icon { get; set; }

    public string Url { get; set; }

    public int? ParentMenuId { get; set; }

    public bool? HasSubMenu { get; set; }

    public bool Status { get; set; }
}
