using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class MedicalSupplyUsage
{
    public Guid IncidentId { get; set; }

    public Guid SupplyId { get; set; }

    public int QuantityUsed { get; set; }

    public DateTime UsageDate { get; set; }

    public string? Notes { get; set; }

    public virtual MedicalIncident Incident { get; set; } = null!;

    public virtual MedicalSupply Supply { get; set; } = null!;
}
