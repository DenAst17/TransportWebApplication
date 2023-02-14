using System;
using System.Collections.Generic;

namespace TransportWebApplication.Models;

public partial class Auto
{
    public long Id { get; set; }

    public long ModelId { get; set; }

    public string Color { get; set; } = null!;

    public string Vin { get; set; } = null!;

    public string RegisterCode { get; set; } = null!;

    public virtual ICollection<AutoOwner> AutoOwners { get; } = new List<AutoOwner>();

    public virtual Model Model { get; set; } = null!;
}
