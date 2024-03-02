using eSanjeevaniIcu.Data.ExtendedEntities;
using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal
{
    public class CommonFunctions
    {
        private readonly eSanjeevaniIcuDbContext _context;
        public CommonFunctions(eSanjeevaniIcuDbContext appcontext)
        {
            _context = appcontext;
        }
    }
}
