using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Entities.Concrete;
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
        #region fields
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;
        private readonly IProductPriceService _productPriceService;
        private static List<ProductModel> _products;
        #endregion

        #region ctor
        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration configuration,
            IProductService productService,
            IProductPriceService productPriceService)
        {
            _logger = logger;
            _configuration = configuration;
            _productService = productService;
            _productPriceService = productPriceService;
        }
        #endregion

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
            _chromeOptions.AddArgument("--headless");


            using IWebDriver driver = new ChromeDriver(options: _chromeOptions);


            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            driver.Navigate().GoToUrl("https://www.migros.com.tr/meyve-sebze-c-2");
            js.ExecuteScript("window.scrollTo(0, 1000)");
            Thread.Sleep(1000);
            int pageCount = Convert.ToInt32(Math.Ceiling(double.Parse(driver.FindElement(By.CssSelector("div > div.filter__header > div")).Text.Split(" ")[0]) / 30));
            string url = driver.Url;
            _products = new();
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
                    var model = new ProductModel { Name = name, Image = image, Price = price };
                    _products.Add(model);

                }
            }

            SaveDatabase(_products);

            driver.Close();
            ViewBag.ProductCount = _products.Count;

            stopwatch.Stop();
            ViewBag.ElapsedTime = stopwatch.ElapsedMilliseconds;

            return View(model: _products);
        }

        private void SaveDatabase(List<ProductModel> products)
        {
            foreach (var model in products)
            {
                _productService.Add(product: new Product { Image = model.Image, Name = model.Name });
                var entity = _productService.GetByImageSource(model.Image);
                var productPrice = new ProductPrice { SavedDate = DateTime.Now, Price = double.Parse(model.Price.Replace("TL", "").Replace(",", ".")), ProductId = entity.Data.Id };
                _productPriceService.Add(productPrice: productPrice);
            }
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
