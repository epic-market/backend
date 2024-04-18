using AutoMapper;
using EpicMarket.Data.Models;
using EpicMarket.Entities;

namespace EpicMarket.Business.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AddressDto, Address>();
            CreateMap<ProductsDto, Catalog>();
            CreateMap<Catalog, ProductsDto>();
        }
    }
}
