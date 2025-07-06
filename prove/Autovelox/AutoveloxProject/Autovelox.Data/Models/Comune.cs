using System;
using System.Collections.Generic;

namespace Autovelox.Data.Models;

public partial class Comune
{
    public int IdComune { get; set; }

    public int? IdProvincia { get; set; }

    public string? CodiceComune { get; set; }

    public string? Denominazione { get; set; }

    public string? CodiceCatastale { get; set; }

    public int? CapoluogoProvincia { get; set; }

    public string? ZonaAltimetrica { get; set; }

    public int? AltitudineCentro { get; set; }

    public int? ComuneLitoraneo { get; set; }

    public string? ComuneMontano { get; set; }

    public string? SuperficieTerritoriale { get; set; }

    public int? Popolazione2001 { get; set; }

    public int? Popolazione2011 { get; set; }

    public virtual ICollection<Mappa> Mappe { get; set; } = new List<Mappa>();

    public virtual Provincia? IdProvinciaNavigation { get; set; }
}
