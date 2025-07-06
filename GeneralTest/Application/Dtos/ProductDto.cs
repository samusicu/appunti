using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfDbFirst.Models;

namespace Application.Dtos
{
    public class ProductDto
    {
        public string ProductName { get; set; } = null!;

        public string Brand { get; set; }

        public string Category { get; set; }

        public short ModelYear { get; set; }

        public decimal ListPrice { get; set; }
    }
}
