using System;
using System.Collections.Generic;
using SchoolMedicalManagementSystem.Enum;

namespace BusinessObjects;

public partial class Campaign
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }

    public int? Type { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public string TypeDisplay
    {
        get
        {
            return ((CampaignType)Type).ToString() switch
            {
                "Vaccination" => "Tiêm chủng",
                "HealthCheckup" => "Khám sức khỏe",
                _ => "Không xác định"
            };
        }
    }

    public string StatusDisplay
    {
        get
        {
            return ((CampaignStatus)Status).ToString() switch
            {
                "Planned" => "Đã lên kế hoạch",
                "InProgress" => "Đang thực hiện",
                "Completed" => "Đã hoàn thành",
                "Cancelled" => "Đã hủy",
                _ => "Không xác định"
            };
        }
    }

    public virtual ICollection<ConsentForm> ConsentForms { get; set; } = new List<ConsentForm>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<User> MedicalStaffs { get; set; } = new List<User>();
}
