using Autovelox.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autovelox.Application.Dtos
{
    public class DettagliMappaDto
    {
        public int Id { get; set; }

        public string? Nome { get; set; }

        public string? Comune { get; set; }

        public string? SiglaProvincia { get; set; }

        public string? Regione { get; set; }

        public string? RipartizioneGeografica { get; set; }
        
        public int AnnoInserimento { get; set; }

        public DateTime DataOraInserimento { get; set; }

        public double IdentificatoreOpenStreetMap { get; set; }

        public double Longitudine { get; set; }

        public double Latitudine { get; set; }

    }
}
