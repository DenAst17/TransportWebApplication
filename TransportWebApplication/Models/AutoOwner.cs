using System;
using System.Collections.Generic;

namespace TransportWebApplication.Models;

public partial class AutoOwner
{
    public long Id { get; set; }

    public long AutoId { get; set; }

    public long OwnerId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string IncidentsInfo { get; set; } = null!;

    public virtual Auto Auto { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;
}
