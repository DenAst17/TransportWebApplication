using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransportWebApplication.Models;

public partial class Auto
{
    public long Id { get; set; }

    public long ModelId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Колір")]
    public string Color { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "VIN-код")]
    public string Vin { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Код реєстрації")]
    public string RegisterCode { get; set; } = null!;

    public virtual ICollection<AutoOwner> AutoOwners { get; } = new List<AutoOwner>();

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Модель")]
    public virtual Model Model { get; set; } = null!;
}
