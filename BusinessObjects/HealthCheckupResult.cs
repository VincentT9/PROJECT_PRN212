namespace BusinessObjects;

public partial class HealthCheckupResult
{
    public Guid Id { get; set; }

    public Guid? ScheduleDetailId { get; set; }

    public float? Height { get; set; }

    public float? Weight { get; set; }

    public string? VisionLeftResult { get; set; }

    public string? VisionRightResult { get; set; }

    public string? HearingLeftResult { get; set; }

    public string? HearingRightResult { get; set; }

    public float? BloodPressureSys { get; set; }

    public float? BloodPressureDia { get; set; }

    public float? HeartRate { get; set; }

    public string? DentalCheckupResult { get; set; }

    public string? OtherResults { get; set; }

    public string? AbnormalSigns { get; set; }

    public string? Recommendations { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual MedicalConsultation? MedicalConsultation { get; set; }

    public virtual ScheduleDetail? ScheduleDetail { get; set; }
}
