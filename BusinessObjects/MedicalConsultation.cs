using SchoolMedicalManagementSystem.Enum;

namespace BusinessObjects;

public partial class MedicalConsultation
{
    public Guid Id { get; set; }

    public Guid? HealthCheckupResultId { get; set; }

    public Guid? VaccinationResultId { get; set; }

    public Guid StudentId { get; set; }

    public Guid MedicalStaffId { get; set; }

    public DateTime ScheduledDate { get; set; }

    public string? ConsultationNotes { get; set; }

    public ConsultantStatus Status { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual HealthCheckupResult? HealthCheckupResult { get; set; }

    public virtual User MedicalStaff { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual VaccinationResult? VaccinationResult { get; set; }
}
