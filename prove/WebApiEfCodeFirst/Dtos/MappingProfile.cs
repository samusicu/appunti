using AutoMapper;
using WebApiEfCodeFirst.Models;

namespace WebApiEfCodeFirst.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Product, ProductDto>().ForMember(dest => dest.Prezzo, opt => opt.MapFrom(src => src.Price));
            CreateMap<ProductDto, Product>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Prezzo));
        }
    }
}
