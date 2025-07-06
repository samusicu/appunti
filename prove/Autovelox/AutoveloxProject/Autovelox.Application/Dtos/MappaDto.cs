using Autovelox.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autovelox.Application.Dtos
{
    public class MappaDto
    {
        public int Id { get; set; }

        public string? Nome { get; set; }

        public double Longitudine { get; set; }

        public double Latitudine { get; set; }

    }
}
