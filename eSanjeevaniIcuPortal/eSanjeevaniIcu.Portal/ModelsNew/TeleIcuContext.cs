using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eSanjeevaniIcu.Portal.ModelsNew;

public partial class TeleIcuContext : DbContext
{
    public TeleIcuContext()
    {
    }

    public TeleIcuContext(DbContextOptions<TeleIcuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbSignalRcon> TbSignalRcons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=13.211.23.202,1433;Database=teleICU;user id=Anoop;password=AnoopGo2024;;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbSignalRcon>(entity =>
        {
            entity.HasKey(e => e.Srcid);

            entity.ToTable("Tb_SignalRCon");

            entity.Property(e => e.Srcid).HasColumnName("SRCId");
            entity.Property(e => e.ConnectionId)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
