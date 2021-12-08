﻿using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Business.ValidationRules.FluentValidation;
using PriceMonitoring.Core.CrossCuttingConcerns.FluentValidation;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
using PriceMonitoring.WebUI.EmailService;
using PriceMonitoring.WebUI.Models;
using System;
using System.IO;

namespace PriceMonitoring.WebUI.Controllers
{
    public class UserController : Controller
    {
        #region fields

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserController> _logger;


        #endregion

        #region ctor
        public UserController(IUserService userService,
                                IMapper mapper,
                                IEmailSender emailSender, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _emailSender = emailSender;
            _logger = logger;
        }
        #endregion

        #region methods
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
            _logger.LogInformation($"Called register method in {DateTime.Now.ToShortTimeString()} ");
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
                    _logger.LogInformation($"User is registered {DateTime.Now.ToShortTimeString()}. User: {user.FirstName + " " + user.LastName + " " + user.Email} ");
                    string url = Url.Action("ConfirmAccount", "User", new { email = user.Email, token = code });
                    string confirmFilePath = Directory.GetCurrentDirectory() + @"\Views\Shared\ConfirmPage.html";
                    StreamReader str = new StreamReader(confirmFilePath);
                    string mailText = str.ReadToEnd();
                    mailText = mailText.Replace("[username]", model.FirstName).Replace("[email]", model.Email).Replace("href=''", $"href=https://localhost:44396{url}");
                    var message = new Message(to: model.Email, subject: "Confirm Account", content: mailText);
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
            var result = ValidationTool.Validate(new UserLoginValidator(), loginDto);
            result.AddToModelState(ModelState, null);
            if (result.IsValid)
            {
                var user = _userService.GetByEmail(loginDto.Email);
                if (user.Success && user.Data.IsConfirm)
                {
                    var passwordVerified = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Data.Password);
                    if (passwordVerified)
                    {
                        _logger.LogInformation($"User is logged in {DateTime.Now.ToShortTimeString()} User email: {user.Data.Email}");
                        SessionModel.CreateUserSession(user: user.Data, httpContext: HttpContext);
                        return RedirectToAction(nameof(Index), "Home");
                    }
                    else
                    {
                        _logger.LogInformation($"User is not logged in {DateTime.Now.ToShortTimeString()} User email: {user.Data.Email}");
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

        [HttpPost]
        public IActionResult LoginWithDemoAccount()
        {
            string email = "demo@demo.com";
            var user = _userService.GetByEmail(email: email);
            if (user.Success && user.Data.IsConfirm)
            {
                var passwordVerified = BCrypt.Net.BCrypt.Verify("DemoTestPassword123_", user.Data.Password);
                if (passwordVerified)
                {
                    _logger.LogInformation($"Demouser is logged in {DateTime.Now.ToShortTimeString()} Demouser email: {user.Data.Email}");
                    SessionModel.CreateUserSession(user: user.Data, httpContext: HttpContext);
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    _logger.LogInformation($"Demouser is not logged in {DateTime.Now.ToShortTimeString()} Demouser email: {user.Data.Email}");
                    ViewData["Message"] = "Wrong password or email!";
                }
            }
            else
            {
                ViewData["Message"] = "Your account is not confirmed!";
            }

            return View("Login");
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
                    _logger.LogInformation($"User account is confirmed in {DateTime.Now.ToShortTimeString()} User email: {user.Data.Email}");
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
    #endregion
}
