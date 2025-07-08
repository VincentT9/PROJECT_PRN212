using SchoolMedicalManagementSystem.Enum;

namespace BusinessObjects;

public partial class MedicationRequest
{
    public Guid Id { get; set; }

    public Guid? StudentId { get; set; }

    public string? MedicationName { get; set; }

    public int? Dosage { get; set; }

    public int? NumberOfDayToTake { get; set; }

    public string? Instructions { get; set; }

    public string? ImagesMedicalInvoice { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public RequestStatus Status { get; set; }

    public Guid? MedicalStaffId { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<MedicalDiary> MedicalDiaries { get; set; } = new List<MedicalDiary>();

    public virtual User? MedicalStaff { get; set; }

    public virtual Student? Student { get; set; }
}
