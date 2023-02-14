using System;
using System.Collections.Generic;

namespace TransportWebApplication.Models;

public partial class Owner
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public virtual ICollection<AutoOwner> AutoOwners { get; } = new List<AutoOwner>();
}
