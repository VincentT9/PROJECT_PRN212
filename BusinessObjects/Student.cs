using SchoolMedicalManagementSystem.Enum;

namespace BusinessObjects;

public partial class Student
{
    public Guid Id { get; set; }

    public Guid? ParentId { get; set; }

    public string? StudentCode { get; set; }

    public string? FullName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public string? Class { get; set; }

    public string? SchoolYear { get; set; }

    public string? Image { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<ConsentForm> ConsentForms { get; set; } = new List<ConsentForm>();

    public virtual HealthRecord? HealthRecord { get; set; }

    public virtual ICollection<MedicalConsultation> MedicalConsultations { get; set; } = new List<MedicalConsultation>();

    public virtual ICollection<MedicalDiary> MedicalDiaries { get; set; } = new List<MedicalDiary>();

    public virtual ICollection<MedicalIncident> MedicalIncidents { get; set; } = new List<MedicalIncident>();

    public virtual ICollection<MedicationRequest> MedicationRequests { get; set; } = new List<MedicationRequest>();

    public virtual User? Parent { get; set; }

    public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; } = new List<ScheduleDetail>();
}
