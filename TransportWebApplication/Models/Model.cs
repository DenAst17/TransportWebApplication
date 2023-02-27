using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransportWebApplication.Models;

public partial class Model
{

    public long Id { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва моделі")]
    public string Name { get; set; } = null!;

    public virtual ICollection<Auto> Autos { get; } = new List<Auto>();
}
