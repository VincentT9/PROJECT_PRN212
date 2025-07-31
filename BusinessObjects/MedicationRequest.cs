using SchoolMedicalManagementSystem.Enum;
using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class MedicationRequest
{
    public Guid? Id { get; set; }
    public Guid? StudentId { get; set; }
    public string? MedicationName { get; set; }
    public int? Dosage { get; set; }
    public int? NumberOfDayToTake { get; set; }
    public string? Instructions { get; set; }
    public List<string>? ImagesMedicalInvoice { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? StartDateVN => StartDate?.AddHours(7);
    public DateTime? EndDateVN => EndDate?.AddHours(7);
    public RequestStatus Status { get; set; }
    public Guid? MedicalStaffId { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public DateTime CreateAtVN => CreateAt.AddHours(7);

    public DateTime UpdateAtVN => UpdateAt.AddHours(7);

    public virtual ICollection<MedicalDiary> MedicalDiaries { get; set; } = new List<MedicalDiary>();

    public virtual User? MedicalStaff { get; set; }

    public virtual Student? Student { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string? StudentName { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string? StatusText { get; set; }
}
