using System;
using System.Collections.Generic;

namespace Autovelox.Data.Models;

public partial class Regione
{
    public int Id { get; set; }

    public int IdRipartizioneGeografica { get; set; }

    public string Denominazione { get; set; } = null!;

    public virtual RipartizioneGeografica IdRipartizioneGeograficaNavigation { get; set; } = null!;

    public virtual ICollection<Provincia> Province { get; set; } = new List<Provincia>();
}
