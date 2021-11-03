using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PriceMonitoring.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private static List<ProductModel> _products;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var stopwatch = Stopwatch.StartNew();
            var _chromeOptions = new ChromeOptions();
            var userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.50 Safari/537.36";
            _chromeOptions.AddArgument("--window-size=1920,1080");
            _chromeOptions.AddArgument("--start-maximized");
            _chromeOptions.AddArgument($"user_agent={userAgent}");
            _chromeOptions.AddArgument("--ignore-certificate-errors");
            _chromeOptions.AddArgument("no-sandbox");
            _chromeOptions.AddArgument("--disable-gpu");
            //_chromeOptions.AddArgument("--headless");
            using IWebDriver driver = new ChromeDriver(options: _chromeOptions);

            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            for (int j = 1; j <= 4; j++)
            {
                driver.Navigate().GoToUrl("https://www.migros.com.tr");
                js.ExecuteScript("window.scrollTo(0, 1000)");
                var element = driver.FindElement(By.CssSelector($"div.categories-wrapper.ng-tns-c369-4 > div:nth-child({j}) > a"));
                element.Click();
                Thread.Sleep(1000);
                int pageCount = Convert.ToInt32(Math.Ceiling(double.Parse(driver.FindElement(By.CssSelector("div > div.filter__header > div")).Text.Split(" ")[0]) / 30));
                string url = driver.Url;
                _products = new List<ProductModel>();
                for (int i = 1; i <= pageCount; i++)
                {
                    string newUrl = url + $"?sayfa={i}";
                    driver.Navigate().GoToUrl(newUrl);
                    Thread.Sleep(500);
                    js.ExecuteScript("window.scrollTo(0, 750)");
                    js.ExecuteScript("window.scrollTo(750, 1750)");
                    var products = driver.FindElements(by: By.CssSelector("div[class*='product-cards'] *[class*='mdc-card']"));
                    foreach (var item in products)
                    {
                        string name = item.FindElement(By.CssSelector("a[class*='product-name']")).Text;
                        string price = item.FindElement(By.CssSelector("span[class*='amount']")).Text;
                        string image = item.FindElement(By.TagName("img")).GetAttribute("src");
                        var model = new ProductModel { Name = name, Price = price, Image = image };
                        _products.Add(model);
                    }
                }
            }




            driver.Close();
            _products = new();
            
            ViewBag.ProductCount = _products.Count;

            stopwatch.Stop();
            ViewBag.ElapsedTime = stopwatch.ElapsedMilliseconds;

            return View(model: _products);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
