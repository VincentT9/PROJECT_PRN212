using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class MedicalIncident
{
    public Guid Id { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? MedicalStaffId { get; set; }

    public int IncidentType { get; set; }

    public DateTime IncidentDate { get; set; }

    public string? Description { get; set; }

    public string? ActionsTaken { get; set; }

    public string? Outcome { get; set; }

    public int Status { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual User? MedicalStaff { get; set; }

    public virtual ICollection<MedicalSupplyUsage> MedicalSupplyUsages { get; set; } = new List<MedicalSupplyUsage>();

    public virtual Student? Student { get; set; }
    // Các thuộc tính hiển thị cho DataGrid
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string? IncidentTypeDisplay { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string? DescriptionDisplay { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string? ActionsTakenDisplay { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string? OutcomeDisplay { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string? StatusDisplay { get; set; }
}
