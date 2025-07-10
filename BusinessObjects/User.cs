using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class User
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public int UserRole { get; set; }

    public string? Image { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<MedicalConsultation> MedicalConsultations { get; set; } = new List<MedicalConsultation>();

    public virtual ICollection<MedicalIncident> MedicalIncidents { get; set; } = new List<MedicalIncident>();

    public virtual ICollection<MedicationRequest> MedicationRequests { get; set; } = new List<MedicationRequest>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
}
