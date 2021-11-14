using AutoMapper;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;

namespace PriceMonitoring.Api.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();
            CreateMap<Product, ProductListDto>().ReverseMap();
        }
    }
}
