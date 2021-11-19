using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Business.ValidationRules.FluentValidation;
using PriceMonitoring.Core.CrossCuttingConcerns.FluentValidation;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
using PriceMonitoring.WebUI.EmailService;
using PriceMonitoring.WebUI.Models;
using System;

namespace PriceMonitoring.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public UserController(IUserService userService,
                                IMapper mapper,
                                IEmailSender emailSender)
        {
            _userService = userService;
            _mapper = mapper;
            _emailSender = emailSender;
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
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                string code = Guid.NewGuid().ToString();
                user.Password = passwordHash;
                user.Token = code;
                var addedResult = _userService.Add(user: user);
                if (addedResult.Success)
                {
                    string url = Url.Action("ConfirmAccount", "User", new { email = user.Email, token = code });
                    var message = new Message(to: model.Email, subject: "Confirm Account", content: $"{model.FirstName} {model.Email} 'https://localhost:44396{url}'");
                    _emailSender.SendEmail(message: message);
                    return RedirectToAction(nameof(ConfirmAccount));
                }
                ViewData["Message"] = addedResult.Message;
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
            // Manuel validation
            var result = ValidationTool.Validate(new UserLoginValidator(), loginDto); ;
            result.AddToModelState(ModelState, null);
            if (result.IsValid)
            {
                var user = _userService.GetByEmail(loginDto.Email);
                if (user.Success && user.Data.IsConfirm)
                {
                    var passwordVerified = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Data.Password);
                    if (passwordVerified)
                    {
                        SessionModel.CreateUserSession(user: user.Data, httpContext: HttpContext);
                        return RedirectToAction(nameof(Index), "Home");
                    }
                    else
                    {
                        ViewData["Message"] = "Wrong password or email!";
                    }
                }
                else
                {
                    ViewData["Message"] = "Your account is not confirmed!";
                }


            }

            return View();
        }

        public IActionResult Logout()
        {
            SessionModel.ClearUserSession(httpContext: HttpContext);
            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult ConfirmAccount(string email, string token)
        {
            var user = _userService.GetByEmail(email: email);
            if (user.Success)
            {
                if (user.Data.Token == token)
                {
                    ViewData["SuccessMessage"] = "Your account is confirmed!";
                    user.Data.IsConfirm = true;
                    _userService.Update(user: user.Data);
                }
            }
            else
            {
                ViewData["WarningMessage"] = "Your account is not confirmed!";
            }

            return View();
        }
    }
}
