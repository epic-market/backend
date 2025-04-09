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
			CreateMap<FAQ, FaqDto>();
            CreateMap<FaqDto, FAQ>();
            CreateMap<FaqCategoryDto, FAQCategory>();
            CreateMap<FAQCategory, FaqCategoryDto>();
            CreateMap<OrderDetailsDto, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailsDto>();
            CreateMap<CategoriesDto, CatalogCategory>();
            CreateMap<CatalogCategory, CategoriesDto>();
        }
    }
}
