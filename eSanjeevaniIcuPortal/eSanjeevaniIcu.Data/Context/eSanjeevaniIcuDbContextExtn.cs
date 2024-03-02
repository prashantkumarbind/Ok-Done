using System;
using eSanjeevaniIcu.Data.ExtendedEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities
{
    public partial class eSanjeevaniIcuDbContext : DbContext
    {
        public virtual DbSet<UserCountEntity> UserCountEntity { get; set; }
        public virtual DbSet<StringEntity> StringEntity { get; set; }
        public virtual DbSet<MenuMasterEntity> MenuMasterEntity { get; set; }
        public virtual DbSet<GroupPermissionEntity> GroupPermissionEntity { get; set; }
        public virtual DbSet<MenusWithPermissionEntity> MenusWithPermissionEntity { get; set; }
    }
}
