using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.WebUI.Models;
using PriceMonitoring.WebUI.Models.GroceryStore;
using PriceMonitoring.WebUI.Models.Selenium.Chrome;
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
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration configuration,
            IProductService productService,
            IProductPriceService productPriceService, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _productService = productService;
            _productPriceService = productPriceService;
            _mapper = mapper;
        }
        #endregion

        public IActionResult Index()
        {
            var stopwatch = Stopwatch.StartNew();
            //var products = new Migros().GetProducts(url: "https://www.migros.com.tr/meyve-sebze-c-2");

            var products = _productService.GetProductsWithPrice().Data.ToList();
            var productsModel = new List<ProductModel>();
            //SaveDatabase(products);
            products.ForEach(x => productsModel.Add(_mapper.Map<ProductModel>(x)));
            ViewBag.ProductCount = products.Count();
            stopwatch.Stop();
            ViewBag.ElapsedTime = stopwatch.ElapsedMilliseconds;

            return View(model: productsModel);
        }

        private void SaveDatabase(List<ProductModel> products)
        {
            foreach (var model in products)
            {
                _productService.Add(product: new Product { Image = model.Image, Name = model.Name });
                var entity = _productService.GetByImageSource(model.Image.ToString());
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
