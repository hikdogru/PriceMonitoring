using AutoMapper;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.WebUI.Models;
using PriceMonitoring.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // ReverseMap => Two way mapping
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<Product, ProductPriceViewModel>().ReverseMap();
        }
    }
}
