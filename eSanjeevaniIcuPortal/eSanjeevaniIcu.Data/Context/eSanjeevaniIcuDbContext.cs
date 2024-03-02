using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;

public partial class eSanjeevaniIcuDbContext : DbContext
{
    public eSanjeevaniIcuDbContext()
    {
    }

    public eSanjeevaniIcuDbContext(DbContextOptions<eSanjeevaniIcuDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PTable> PTables { get; set; }

    public virtual DbSet<PatientDatum> PatientData { get; set; }

    public virtual DbSet<TbBedMaster> TbBedMasters { get; set; }

    public virtual DbSet<TbCityMaster> TbCityMasters { get; set; }

    public virtual DbSet<TbCoeMaster> TbCoeMasters { get; set; }

    public virtual DbSet<TbDistrictMaster> TbDistrictMasters { get; set; }

    public virtual DbSet<TbDoctor> TbDoctors { get; set; }

    public virtual DbSet<TbGroupMaster> TbGroupMasters { get; set; }

    public virtual DbSet<TbGroupPermission> TbGroupPermissions { get; set; }

    public virtual DbSet<TbHubHospital> TbHubHospitals { get; set; }

    public virtual DbSet<TbIcu> TbIcus { get; set; }

    public virtual DbSet<TbIndianState> TbIndianStates { get; set; }

    public virtual DbSet<TbMappingMaster> TbMappingMasters { get; set; }

    public virtual DbSet<TbMenuMaster> TbMenuMasters { get; set; }

    public virtual DbSet<TbNurse> TbNurses { get; set; }

    public virtual DbSet<TbPatient> TbPatients { get; set; }

    public virtual DbSet<TbPatientCommunication> TbPatientCommunications { get; set; }

    public virtual DbSet<TbPatientDetail> TbPatientDetails { get; set; }

    public virtual DbSet<TbPatientMaster> TbPatientMasters { get; set; }

    public virtual DbSet<TbPatientStatus> TbPatientStatuses { get; set; }

    public virtual DbSet<TbQualificationMaster> TbQualificationMasters { get; set; }

    public virtual DbSet<TbSpecialistMaster> TbSpecialistMasters { get; set; }

    public virtual DbSet<TbSpecialityMaster> TbSpecialityMasters { get; set; }

    public virtual DbSet<TbSpokeHospital> TbSpokeHospitals { get; set; }

    public virtual DbSet<TbState> TbStates { get; set; }

    public virtual DbSet<TbStateAdmin> TbStateAdmins { get; set; }

    public virtual DbSet<TbUserDetail> TbUserDetails { get; set; }

    public virtual DbSet<TbUserGroupMapping> TbUserGroupMappings { get; set; }

    public virtual DbSet<TbUserMaster> TbUserMasters { get; set; }
    public virtual DbSet<TbUserActivity> TbUserActivity { get; set; }
    public virtual DbSet<TbSignalRcon> TbSignalRcons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=13.211.23.202,1433;Database=teleICU;user id=Anoop;password=AnoopGo2024;TrustServerCertificate=True;", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ID");

            entity.ToTable("P_Table");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(dateadd(minute,(-330),getdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedOnUtc)
                .HasDefaultValueSql("(dateadd(minute,(-660),getdate()))")
                .HasColumnType("datetime")
                .HasColumnName("CreatedOnUTC");
            entity.Property(e => e.PDia).HasColumnName("p_dia");
            entity.Property(e => e.PFirstName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("p_first_name");
            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.PLastName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("p_last_name");
            entity.Property(e => e.PMean).HasColumnName("p_mean");
            entity.Property(e => e.POxim).HasColumnName("p_oxim");
            entity.Property(e => e.PPulseRate).HasColumnName("p_pulse_rate");
            entity.Property(e => e.PSys).HasColumnName("p_sys");
            entity.Property(e => e.TimeStamp)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PatientDatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("patient_data");

            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.PDia).HasColumnName("p_dia");
            entity.Property(e => e.PFirstName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("p_first_name");
            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.PLastName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("p_last_name");
            entity.Property(e => e.PMean).HasColumnName("p_mean");
            entity.Property(e => e.POxim).HasColumnName("p_oxim");
            entity.Property(e => e.PPulseRate).HasColumnName("p_pulse_rate");
            entity.Property(e => e.PSys).HasColumnName("p_sys");
        });

        modelBuilder.Entity<TbBedMaster>(entity =>
        {
            entity.HasKey(e => e.BedId);

            entity.ToTable("Tb_BedMaster");

            entity.Property(e => e.BedNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.BedType)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbCityMaster>(entity =>
        {
            entity.HasKey(e => e.CityId);

            entity.ToTable("Tb_CityMaster");

            entity.Property(e => e.CityCode)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CityName).HasMaxLength(100);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbCoeMaster>(entity =>
        {
            entity.HasKey(e => e.CoeId);

            entity.ToTable("Tb_CoeMaster");

            entity.Property(e => e.AddressLine1)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValueSql("('Guest')");
            entity.Property(e => e.AddressLine2).HasMaxLength(100);
            entity.Property(e => e.CityId).HasDefaultValueSql("((1))");
            entity.Property(e => e.CoeCode)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CoeName).HasMaxLength(100);
            entity.Property(e => e.ContactNumber)
                .IsRequired()
                .HasMaxLength(15)
                .HasDefaultValueSql("('0')");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Designation).HasMaxLength(30);
            entity.Property(e => e.DistrictId).HasDefaultValueSql("((1))");
            entity.Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('guest@guest.com')");
            entity.Property(e => e.FirstName).HasMaxLength(30);
            entity.Property(e => e.Honor).HasMaxLength(5);
            entity.Property(e => e.LastName).HasMaxLength(30);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.MiddleName).HasMaxLength(30);
            entity.Property(e => e.PinCode)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValueSql("('0')");
            entity.Property(e => e.PlaceOfWork).HasMaxLength(150);
            entity.Property(e => e.ProfileImage).HasMaxLength(150);
            entity.Property(e => e.StateId).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TbDistrictMaster>(entity =>
        {
            entity.HasKey(e => e.DistrictId);

            entity.ToTable("Tb_DistrictMaster");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DistrictCode)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.DistrictName).HasMaxLength(100);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbDoctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId);

            entity.ToTable("Tb_Doctor");

            entity.Property(e => e.AddressLine1)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.AddressLine2).HasMaxLength(100);
            entity.Property(e => e.ContactNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FacebookProfile).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Language).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.LinkedinProfile).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.PinCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.PlaceOfWork)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.RegistrationNumber).HasMaxLength(50);
            entity.Property(e => e.SurName).HasMaxLength(50);
            entity.Property(e => e.TwitterProfile).HasMaxLength(100);
        });

        modelBuilder.Entity<TbGroupMaster>(entity =>
        {
            entity.HasKey(e => e.GroupId);

            entity.ToTable("Tb_GroupMaster");

            entity.Property(e => e.GroupName)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<TbGroupPermission>(entity =>
        {
            entity.HasKey(e => e.GroupPermissionId);

            entity.ToTable("Tb_GroupPermissions");

            entity.Property(e => e.MenuId).HasComment("It will contains comma separated Menu Ids");
        });

        modelBuilder.Entity<TbHubHospital>(entity =>
        {
            entity.HasKey(e => e.HubHospitalId);

            entity.ToTable("Tb_HubHospital");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.ContactNo).HasMaxLength(100);
            entity.Property(e => e.CreatedOn).HasColumnType("date");
            entity.Property(e => e.HubHospitalName)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("date");
            entity.Property(e => e.Pin).HasColumnName("PIN");
            entity.Property(e => e.State).HasMaxLength(100);
        });

        modelBuilder.Entity<TbIcu>(entity =>
        {
            entity.HasKey(e => e.IcuId);

            entity.ToTable("Tb_Icu");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IcuName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Other)
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TbIndianState>(entity =>
        {
            entity.HasKey(e => e.StateId);

            entity.ToTable("Tb_IndianState");

            entity.Property(e => e.CreatedOn).HasColumnType("date");
            entity.Property(e => e.LastUpdatedOn).HasColumnType("date");
            entity.Property(e => e.StateCode)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.StateName).HasMaxLength(100);
        });

        modelBuilder.Entity<TbMappingMaster>(entity =>
        {
            entity.HasKey(e => new { e.MappingKey, e.MappingValue, e.MappingType }).HasName("PK_MappingMaster");

            entity.ToTable("Tb_MappingMaster");

            entity.Property(e => e.MappingType).HasMaxLength(5);
        });

        modelBuilder.Entity<TbMenuMaster>(entity =>
        {
            entity.HasKey(e => e.MenuId);

            entity.ToTable("Tb_MenuMaster");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.HasSubMenu).HasDefaultValueSql("((0))");
            entity.Property(e => e.Icon).HasMaxLength(100);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Sequence).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Url).HasMaxLength(200);
        });

        modelBuilder.Entity<TbNurse>(entity =>
        {
            entity.HasKey(e => e.NurseId);

            entity.ToTable("Tb_Nurse");

            entity.Property(e => e.AddressLine1)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.AddressLine2).HasMaxLength(100);
            entity.Property(e => e.ContactNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FacebookProfile).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Language).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.LinkedinProfile).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.PinCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.RegistrationNumber).HasMaxLength(50);
            entity.Property(e => e.SurName).HasMaxLength(50);
            entity.Property(e => e.TwitterProfile).HasMaxLength(100);
        });

        modelBuilder.Entity<TbPatient>(entity =>
        {
            entity.HasKey(e => e.PatientId);

            entity.ToTable("Tb_Patient");

            entity.Property(e => e.AddressLine1)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.AddressLine2).HasMaxLength(100);
            entity.Property(e => e.AdmitDate).HasColumnType("datetime");
            entity.Property(e => e.BloodGroup)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.ContactNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.PinCode)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TbPatientCommunication>(entity =>
        {
            entity.HasKey(e => e.PatientCommunicationId);

            entity.ToTable("Tb_PatientCommunication");

            entity.Property(e => e.Comment)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbPatientDetail>(entity =>
        {
            entity.HasKey(e => e.PatientDetailId);

            entity.ToTable("Tb_PatientDetail");

            entity.Property(e => e.BloodPressureDia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("BloodPressure_DIA");
            entity.Property(e => e.BloodPressureSys)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("BloodPressure_SYS");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.HeartRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.OxygenSaturationSpo2)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("OxygenSaturation_Spo2");
            entity.Property(e => e.RrRespiratoryRate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("RR_RespiratoryRate");
            entity.Property(e => e.Temperature).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TbPatientMaster>(entity =>
        {
            entity.HasKey(e => e.PatientId);

            entity.ToTable("Tb_PatientMaster");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.AdmissionDate).HasColumnType("date");
            entity.Property(e => e.BedNo).HasMaxLength(10);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.ContactNo).HasMaxLength(100);
            entity.Property(e => e.CreatedOn).HasColumnType("date");
            entity.Property(e => e.LastUpdatedOn).HasColumnType("date");
            entity.Property(e => e.PatientCode)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PatientName)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.Pin).HasColumnName("PIN");
            entity.Property(e => e.State).HasMaxLength(100);
        });

        modelBuilder.Entity<TbPatientStatus>(entity =>
        {
            entity.HasKey(e => e.PatientStatusId);

            entity.ToTable("Tb_PatientStatus");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.PatientStatusName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TbQualificationMaster>(entity =>
        {
            entity.HasKey(e => e.QualificationId).HasName("PK_Tb_QuallificationMaster");

            entity.ToTable("Tb_QualificationMaster");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.QualificationName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TbSpecialistMaster>(entity =>
        {
            entity.HasKey(e => e.SpecialistId);

            entity.ToTable("Tb_SpecialistMaster");

            entity.Property(e => e.AddressLine1)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.AddressLine2).HasMaxLength(100);
            entity.Property(e => e.ContactNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FacebookProfile).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Language).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.LinkedinProfile).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.PinCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.PlaceOfWork)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.RegistrationNumber).HasMaxLength(50);
            entity.Property(e => e.SurName).HasMaxLength(50);
            entity.Property(e => e.TwitterProfile).HasMaxLength(100);
        });

        modelBuilder.Entity<TbSpecialityMaster>(entity =>
        {
            entity.HasKey(e => e.SpecialityId);

            entity.ToTable("Tb_SpecialityMaster");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.SpecialityName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TbSpokeHospital>(entity =>
        {
            entity.HasKey(e => e.SpokeHospitalId);

            entity.ToTable("Tb_SpokeHospital");

            entity.Property(e => e.Address1).HasMaxLength(500);
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ContactNo).HasMaxLength(100);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Pin).HasColumnName("PIN");
            entity.Property(e => e.SpokeHospitalName)
                .IsRequired()
                .HasMaxLength(500);
        });

        modelBuilder.Entity<TbState>(entity =>
        {
            entity.HasKey(e => e.StateId);

            entity.ToTable("Tb_State");

            entity.Property(e => e.CreatedOn).HasColumnType("date");
            entity.Property(e => e.LastUpdatedOn).HasColumnType("date");
            entity.Property(e => e.StateCode)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.StateName).HasMaxLength(100);
        });

        modelBuilder.Entity<TbStateAdmin>(entity =>
        {
            entity.HasKey(e => e.StateAdminId);

            entity.ToTable("Tb_StateAdmin");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Designation).HasMaxLength(100);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PlaceOfWork)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.SurName).HasMaxLength(50);
        });

        modelBuilder.Entity<TbUserDetail>(entity =>
        {
            entity.HasKey(e => e.UserDetailId);

            entity.ToTable("Tb_UserDetail");

            entity.Property(e => e.AddressLine1)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.AddressLine2).HasMaxLength(100);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.FacebookProfile).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Language).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.LinkedinProfile).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.PinCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.PlaceOfWork)
                .HasMaxLength(50);
            entity.Property(e => e.RegistrationNumber).HasMaxLength(50);
            entity.Property(e => e.SurName).HasMaxLength(50);
            entity.Property(e => e.TwitterProfile).HasMaxLength(100);
        });

        modelBuilder.Entity<TbUserGroupMapping>(entity =>
        {
            entity.HasKey(e => e.UserGroupMappingId);

            entity.ToTable("Tb_UserGroupMapping");
        });

        modelBuilder.Entity<TbUserActivity>(entity =>
        {
            entity.HasKey(e => e.UserActivityId);

            entity.ToTable("Tb_UserActivity");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LoginTime).HasColumnType("datetime");
            entity.Property(e => e.LogoutTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });
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
        modelBuilder.Entity<TbUserMaster>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Tb_UserMaster");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.MobileNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
