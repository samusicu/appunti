using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autovelox.Application.Dtos
{
    public class DettagliComuneDto
    {
        public int IdComune { get; set; }
        public string Nome { get; set; }
        public string Provincia { get; set; }
        public string Regione { get; set; }
        public string RipartizioneGeografica {  get; set; }
    }
}
