using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using AutoMapper;
using EfDbFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ProductService(BikeStoresContext context, IMapper mapper)
    {
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var products = await context.Products.Include(p => p.Brand).Include(p => p.Category).ToListAsync();
            return mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
