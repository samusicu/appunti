using AutoMapper;
using Autovelox.Application.Dtos;
using Autovelox.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autovelox.Application.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Mappa, MappaDto>();
        }
    }
}
