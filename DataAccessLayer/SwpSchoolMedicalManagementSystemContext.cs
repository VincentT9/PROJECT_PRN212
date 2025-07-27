using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataAccessLayer;

public partial class SwpSchoolMedicalManagementSystemContext : DbContext
{
    public SwpSchoolMedicalManagementSystemContext()
    {
    }

    public SwpSchoolMedicalManagementSystemContext(DbContextOptions<SwpSchoolMedicalManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<ConsentForm> ConsentForms { get; set; }

    public virtual DbSet<HealthCheckupResult> HealthCheckupResults { get; set; }

    public virtual DbSet<HealthRecord> HealthRecords { get; set; }

    public virtual DbSet<MedicalConsultation> MedicalConsultations { get; set; }

    public virtual DbSet<MedicalDiary> MedicalDiaries { get; set; }

    public virtual DbSet<MedicalIncident> MedicalIncidents { get; set; }

    public virtual DbSet<MedicalSupply> MedicalSupplies { get; set; }

    public virtual DbSet<MedicalSupplyUsage> MedicalSupplyUsages { get; set; }

    public virtual DbSet<MedicationRequest> MedicationRequests { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleDetail> ScheduleDetails { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VaccinationResult> VaccinationResults { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=dpg-d1pkd7bipnbc7384hag0-a.singapore-postgres.render.com;Port=5432;Username=sa;Password=ry3uG1LqjtUruUG3im8on3FOvkALUx5C;Database=schoolmedicalmanagementsystem_xp62;SSL Mode=Require;Trust Server Certificate=true;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasIndex(e => e.AuthorId, "IX_Blogs_AuthorId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Author).WithMany(p => p.Blogs).HasForeignKey(d => d.AuthorId);
        });

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasMany(d => d.MedicalStaffs).WithMany(p => p.Campaigns)
                .UsingEntity<Dictionary<string, object>>(
                    "CampaignUser",
                    r => r.HasOne<User>().WithMany().HasForeignKey("MedicalStaffsId"),
                    l => l.HasOne<Campaign>().WithMany().HasForeignKey("CampaignsId"),
                    j =>
                    {
                        j.HasKey("CampaignsId", "MedicalStaffsId");
                        j.ToTable("CampaignUser");
                        j.HasIndex(new[] { "MedicalStaffsId" }, "IX_CampaignUser_MedicalStaffsId");
                    });
        });

        modelBuilder.Entity<ConsentForm>(entity =>
        {
            entity.HasIndex(e => e.CampaignId, "IX_ConsentForms_CampaignId");

            entity.HasIndex(e => e.StudentId, "IX_ConsentForms_StudentId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Campaign).WithMany(p => p.ConsentForms)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Student).WithMany(p => p.ConsentForms)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<HealthCheckupResult>(entity =>
        {
            entity.HasIndex(e => e.ScheduleDetailId, "IX_HealthCheckupResults_ScheduleDetailId")
                .IsUnique()
                .HasFilter("([ScheduleDetailId] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ScheduleDetail).WithOne(p => p.HealthCheckupResult).HasForeignKey<HealthCheckupResult>(d => d.ScheduleDetailId);
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasIndex(e => e.StudentId, "IX_HealthRecords_StudentId")
                .IsUnique()
                .HasFilter("([StudentId] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Student).WithOne(p => p.HealthRecord).HasForeignKey<HealthRecord>(d => d.StudentId);
        });

        modelBuilder.Entity<MedicalConsultation>(entity =>
        {
            entity.HasIndex(e => e.HealthCheckupResultId, "IX_MedicalConsultations_HealthCheckupResultId")
                .IsUnique()
                .HasFilter("([HealthCheckupResultId] IS NOT NULL)");

            entity.HasIndex(e => e.MedicalStaffId, "IX_MedicalConsultations_MedicalStaffId");

            entity.HasIndex(e => e.StudentId, "IX_MedicalConsultations_StudentId");

            entity.HasIndex(e => e.VaccinationResultId, "IX_MedicalConsultations_VaccinationResultId")
                .IsUnique()
                .HasFilter("([VaccinationResultId] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.HealthCheckupResult).WithOne(p => p.MedicalConsultation).HasForeignKey<MedicalConsultation>(d => d.HealthCheckupResultId);

            entity.HasOne(d => d.MedicalStaff).WithMany(p => p.MedicalConsultations)
                .HasForeignKey(d => d.MedicalStaffId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Student).WithMany(p => p.MedicalConsultations)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.VaccinationResult).WithOne(p => p.MedicalConsultation).HasForeignKey<MedicalConsultation>(d => d.VaccinationResultId);
        });

        modelBuilder.Entity<MedicalDiary>(entity =>
        {
            entity.HasIndex(e => e.MedicationReqId, "IX_MedicalDiaries_MedicationReqId");

            entity.HasIndex(e => e.StudentId, "IX_MedicalDiaries_StudentId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.MedicationReq).WithMany(p => p.MedicalDiaries).HasForeignKey(d => d.MedicationReqId);

            entity.HasOne(d => d.Student).WithMany(p => p.MedicalDiaries).HasForeignKey(d => d.StudentId);
        });

        modelBuilder.Entity<MedicalIncident>(entity =>
        {
            entity.HasIndex(e => e.MedicalStaffId, "IX_MedicalIncidents_MedicalStaffId");

            entity.HasIndex(e => e.StudentId, "IX_MedicalIncidents_StudentId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.MedicalStaff).WithMany(p => p.MedicalIncidents).HasForeignKey(d => d.MedicalStaffId);

            entity.HasOne(d => d.Student).WithMany(p => p.MedicalIncidents).HasForeignKey(d => d.StudentId);
        });

        modelBuilder.Entity<MedicalSupply>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<MedicalSupplyUsage>(entity =>
        {
            entity.HasKey(e => new { e.SupplyId, e.IncidentId });

            entity.HasIndex(e => e.IncidentId, "IX_MedicalSupplyUsages_IncidentId");

            entity.HasOne(d => d.Incident).WithMany(p => p.MedicalSupplyUsages)
                .HasForeignKey(d => d.IncidentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Supply).WithMany(p => p.MedicalSupplyUsages)
                .HasForeignKey(d => d.SupplyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MedicationRequest>(entity =>
        {
            entity.HasIndex(e => e.MedicalStaffId, "IX_MedicationRequests_MedicalStaffId");

            entity.HasIndex(e => e.StudentId, "IX_MedicationRequests_StudentId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.MedicalStaff).WithMany(p => p.MedicationRequests).HasForeignKey(d => d.MedicalStaffId);

            entity.HasOne(d => d.Student).WithMany(p => p.MedicationRequests).HasForeignKey(d => d.StudentId);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasIndex(e => e.CampaignId, "IX_Schedules_CampaignId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Campaign).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ScheduleDetail>(entity =>
        {
            entity.HasIndex(e => e.ScheduleId, "IX_ScheduleDetails_ScheduleId");

            entity.HasIndex(e => e.StudentId, "IX_ScheduleDetails_StudentId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Schedule).WithMany(p => p.ScheduleDetails).HasForeignKey(d => d.ScheduleId);

            entity.HasOne(d => d.Student).WithMany(p => p.ScheduleDetails).HasForeignKey(d => d.StudentId);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(e => e.ParentId, "IX_Students_ParentId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Parent).WithMany(p => p.Students).HasForeignKey(d => d.ParentId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            
            
        });

        modelBuilder.Entity<VaccinationResult>(entity =>
        {
            entity.HasIndex(e => e.ScheduleDetailId, "IX_VaccinationResults_ScheduleDetailId")
                .IsUnique()
                .HasFilter("([ScheduleDetailId] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ScheduleDetail).WithOne(p => p.VaccinationResult).HasForeignKey<VaccinationResult>(d => d.ScheduleDetailId);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Content).IsRequired();

            // Cấu hình mối quan hệ many-to-many với User
            entity.HasMany(d => d.Users).WithMany(p => p.Notifications)
                .UsingEntity<Dictionary<string, object>>(
                    "UserNotification",
                    r => r.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    l => l.HasOne<Notification>().WithMany().HasForeignKey("NotificationId"),
                    j =>
                    {
                        j.HasKey("UserId", "NotificationId");
                        j.ToTable("UserNotification");
                        j.HasIndex(new[] { "NotificationId" }, "IX_UserNotification_NotificationId");
                    });
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    // Handle DateTime conversion for entities
    public override int SaveChanges()
    {
        ConvertLocalDateTimesToUtc();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertLocalDateTimesToUtc();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ConvertLocalDateTimesToUtc()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.ClrType == typeof(DateTime) && property.CurrentValue is DateTime dateTime)
                {
                    if (dateTime.Kind != DateTimeKind.Utc)
                    {
                        property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                }
            }
        }
    }
}
