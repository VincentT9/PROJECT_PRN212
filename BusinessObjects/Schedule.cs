namespace BusinessObjects;

public partial class Schedule
{
    public Guid Id { get; set; }

    public Guid CampaignId { get; set; }

    public DateTime ScheduledDate { get; set; }

    public string? Location { get; set; }

    public string? Notes { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Campaign Campaign { get; set; } = null!;

    public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; } = new List<ScheduleDetail>();
}
