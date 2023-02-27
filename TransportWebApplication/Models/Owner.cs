using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransportWebApplication.Models;

public partial class Owner
{

    public long Id { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ім'я")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Прізвище")]
    public string Surname { get; set; } = null!;

    public virtual ICollection<AutoOwner> AutoOwners { get; } = new List<AutoOwner>();
}
