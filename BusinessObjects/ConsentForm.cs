namespace BusinessObjects;

public partial class ConsentForm
{
    public Guid Id { get; set; }

    public Guid CampaignId { get; set; }

    public Guid StudentId { get; set; }

    public bool IsApproved { get; set; }

    public DateTime ConsentDate { get; set; }

    public string? ReasonForDecline { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Campaign Campaign { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
