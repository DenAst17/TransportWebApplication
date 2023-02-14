using System;
using System.Collections.Generic;

namespace TransportWebApplication.Models;

public partial class Model
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Auto> Autos { get; } = new List<Auto>();
}
