using System;
using System.Collections.Generic;
using SchoolMedicalManagementSystem.Enum;

namespace BusinessObjects;

public partial class MedicalSupply
{
    public Guid Id { get; set; }

    public string? SupplyName { get; set; }

    public int SupplyType { get; set; }

    public string? Unit { get; set; }

    public int? Quantity { get; set; }

    public string? Supplier { get; set; }

    public string? Image { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public string SupplyTypeDisplay
    {
        get
        {
            return ((SupplyType)SupplyType).ToString() switch
            {
                "Medicine" => "Thuốc",
                "MedicalEquipment" => "Thiết bị y tế",
                "FirstAid" => "Dụng cụ sơ cứu",
                "Other" => "Khác",
                _ => "Không xác định"
            };
        }
    }

    public virtual ICollection<MedicalSupplyUsage> MedicalSupplyUsages { get; set; } = new List<MedicalSupplyUsage>();
}
