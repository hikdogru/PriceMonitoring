﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.WebUI.Models;
using PriceMonitoring.WebUI.Models.GroceryStore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        #region methods
        public IActionResult Index()
        {
            var stopwatch = Stopwatch.StartNew();

            //var productsFromMigros = new Migros().GetProducts(url: "https://www.migros.com.tr/meyve-sebze-c-2").ToList();
            //SaveDatabase(products: productsFromMigros);

            //var productsFromA101 = new A101().GetProducts(url: "https://www.a101.com.tr/market/meyve-sebze/").ToList();
            //SaveDatabase(productsFromA101);
            var productsModel = new List<ProductModel>();
            var products = _productService.GetProductsWithPrice().Data.Where(x => x.WebsiteId == 1).Take(12).ToList();
            products.ForEach(x => productsModel.Add(_mapper.Map<ProductModel>(x)));

            //productsFromMigros.ForEach(x => productsModel.Add(_mapper.Map<ProductModel>(x)));
            //productsFromA101.ForEach(x => productsModel.Add(_mapper.Map<ProductModel>(x)));
            ViewBag.ProductCount = productsModel.Count();
            stopwatch.Stop();
            ViewBag.ElapsedTime = stopwatch.ElapsedMilliseconds;
            
            return View(model: productsModel);
        }

        private void SaveDatabase(/*List<ProductModel> products*/)
        {
            //foreach (var model in products)
            //{
            //    _productService.Add(product: new Product { Image = model.Image, Name = model.Name, WebsiteId = model.WebsiteId });
            //    var entity = _productService.GetByImageSource(model.Image.ToString());
            //    var productPrice = new ProductPrice { SavedDate = DateTime.Now, Price = double.Parse(model.Price.Replace("TL", "").Replace(",", ".")), ProductId = entity.Data.Id };
            //    _productPriceService.Add(productPrice: productPrice);
            //}


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

        #endregion
    }
}
