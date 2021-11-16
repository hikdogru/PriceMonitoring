using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Business.ValidationRules.FluentValidation;
using PriceMonitoring.Core.CrossCuttingConcerns.FluentValidation;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
using PriceMonitoring.WebUI.Models;

namespace PriceMonitoring.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,
                                IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserModel model)
        {
            var user = _mapper.Map<User>(model);
            var result = _userService.IsUserValid(user: user);
            result.AddToModelState(ModelState, null);
            if (result.IsValid)
            {
                var addedResult = _userService.Add(user: user);
                if (addedResult.Success)
                {
                    return RedirectToAction(nameof(Index) , "Home");
                }
                ViewData["Message"] = addedResult.Message;
                return View();
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var loginDto = _mapper.Map<UserLoginDto>(model);
            var result = ValidationTool.Validate(new UserLoginValidator(), loginDto); ;
            result.AddToModelState(ModelState, null);
            if (result.IsValid)
            {
                var user = _userService.GetByEmail(loginDto.Email);
                if (user.Success)
                {
                    var confirmPassword = user.Data.Password == loginDto.Password;
                    if (confirmPassword)
                    {
                        return RedirectToAction(nameof(Index), "Home");
                    }
                }
                ViewData["Message"] = "Wrong password or email!";

            }

            return View();
        }
    }
}
