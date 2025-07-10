using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class HealthRecord
{
    public Guid Id { get; set; }

    public Guid? StudentId { get; set; }

    public string? Height { get; set; }

    public string? Weight { get; set; }

    public string? BloodType { get; set; }

    public string? Allergies { get; set; }

    public string? ChronicDiseases { get; set; }

    public string? PastMedicalHistory { get; set; }

    public string? VisionLeft { get; set; }

    public string? VisionRight { get; set; }

    public string? HearingLeft { get; set; }

    public string? HearingRight { get; set; }

    public string? VaccinationHistory { get; set; }

    public string? OtherNotes { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Student? Student { get; set; }
}
