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

    public virtual ICollection<MedicalSupplyUsage> MedicalSupplyUsages { get; set; } = new List<MedicalSupplyUsage>();
}
