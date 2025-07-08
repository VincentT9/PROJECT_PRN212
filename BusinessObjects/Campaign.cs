using SchoolMedicalManagementSystem.Enum;

namespace BusinessObjects;

public partial class Campaign
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public CampaignStatus? Status { get; set; }

    public CampaignType? Type { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<ConsentForm> ConsentForms { get; set; } = new List<ConsentForm>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<User> MedicalStaffs { get; set; } = new List<User>();
}
