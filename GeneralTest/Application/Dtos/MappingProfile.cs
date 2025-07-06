using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EfDbFirst.Models;

namespace Application.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.CategoryName));
        }
    }
}
