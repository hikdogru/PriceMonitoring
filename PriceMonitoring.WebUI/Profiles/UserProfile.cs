using AutoMapper;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
using PriceMonitoring.WebUI.Models;

namespace PriceMonitoring.WebUI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<UserLoginDto, LoginModel>().ReverseMap();
        }
    }
}
