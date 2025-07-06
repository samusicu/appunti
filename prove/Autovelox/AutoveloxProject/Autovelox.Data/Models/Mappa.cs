using System;
using System.Collections.Generic;

namespace Autovelox.Data.Models;

public partial class Mappa
{

    public int Id { get; set; }

    public int? IdComune { get; set; }

    public string? Nome { get; set; }

    public int AnnoInserimento { get; set; }

    public DateTime DataOraInserimento { get; set; }

    public double IdentificatoreOpenStreetMap { get; set; }

    public double Longitudine { get; set; }

    public double Latitudine { get; set; }

    public virtual Comune IdComuneNavigation { get; set; }
}
