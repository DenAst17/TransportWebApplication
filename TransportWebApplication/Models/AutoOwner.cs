using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransportWebApplication.Models;

public partial class AutoOwner
{
    public long Id { get; set; }

    public long AutoId { get; set; }

    public long OwnerId { get; set; }

    /*[Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Початок")]*/
    public DateTime StartDate { get; set; }

    /*[Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Кінець")]*/
    public DateTime EndDate { get; set; }

    /*[Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Інциденти")]*/
    public string IncidentsInfo { get; set; } = null!;

    public virtual Auto Auto { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;
}
