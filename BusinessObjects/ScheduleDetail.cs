using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class ScheduleDetail
{
    public Guid Id { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? ScheduleId { get; set; }

    public DateTime VaccinationDate { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual HealthCheckupResult? HealthCheckupResult { get; set; }

    public virtual Schedule? Schedule { get; set; }

    public virtual Student? Student { get; set; }

    public virtual VaccinationResult? VaccinationResult { get; set; }
}
