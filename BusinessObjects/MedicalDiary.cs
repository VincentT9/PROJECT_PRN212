using SchoolMedicalManagementSystem.Enum;

namespace BusinessObjects;

public partial class MedicalDiary
{
    public Guid Id { get; set; }

    public Guid? MedicationReqId { get; set; }

    public MedicationStatus? Status { get; set; }

    public string? Description { get; set; }

    public Guid? StudentId { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual MedicationRequest? MedicationReq { get; set; }

    public virtual Student? Student { get; set; }
}
