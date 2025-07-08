namespace BusinessObjects;

public partial class VaccinationResult
{
    public Guid Id { get; set; }

    public Guid? ScheduleDetailId { get; set; }

    public string? DosageGiven { get; set; }

    public string? SideEffects { get; set; }

    public string? Notes { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual MedicalConsultation? MedicalConsultation { get; set; }

    public virtual ScheduleDetail? ScheduleDetail { get; set; }
}
