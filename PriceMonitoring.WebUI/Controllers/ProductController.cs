using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.WebUI.Models;
using PriceMonitoring.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.Controllers
{
    public class ProductController : Controller
    {
        #region fields
        private static List<Product> _searchResults;
        private static List<ChartJsonModel> _chartProducts = new();
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        #endregion

        #region methods
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Detail(int id)
        {
            var product = _productService.GetProductWithPriceById(id: id).Data;
            var productPriceModel = _mapper.Map<ProductPriceViewModel>(product);
            var prices = new List<double>();
            var dates = new List<string>();
            productPriceModel.ProductPrice.ToList().ForEach(x => prices.Add(x.Price));
            productPriceModel.ProductPrice.ToList().ForEach(x => dates.Add(x.SavedDate.ToString("dd,MM,yyyy")));
            ViewData["Prices"] = JsonConvert.SerializeObject(prices);
            ViewData["Dates"] = JsonConvert.SerializeObject(dates);
            return View(model: productPriceModel);
        }

        [HttpGet]
        public IActionResult Compare()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string q)
        {
            _chartProducts.Clear();
            if (!string.IsNullOrEmpty(q))
            {
                var result = _productService.Search(q: q);
                if (result.Success)
                {
                    _searchResults = new();
                    _searchResults = result.Data.ToList();
                    return View("Compare", model: result.Data.ToList());
                }
            }
            return View("Compare");
        }


        [HttpPost]
        public IActionResult AddToCompare(int id)
        {
            
            var product = _productService.GetProductWithPriceById(id: id).Data;

            var productPriceModel = _mapper.Map<ProductPriceViewModel>(product);
            var prices = new List<double>();
            var dates = new List<string>();

            productPriceModel.ProductPrice.ToList().ForEach(x => prices.Add(x.Price));

            productPriceModel.ProductPrice.ToList().ForEach(x => dates.Add(x.SavedDate.ToString("dd,MM,yyyy")));

            _chartProducts.Add(new ChartJsonModel { Name = product.Name, Data = prices});
            ViewData["Prices"] = JsonConvert.SerializeObject(prices);
            ViewData["Dates"] = JsonConvert.SerializeObject(dates);
            ViewData["Products"] = JsonConvert.SerializeObject(_chartProducts, Formatting.Indented,
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                                                                        ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
                                                                    });

            string json = ViewData["Products"] as string;

            return View("Compare", model: _searchResults.ToList());
        }

        #endregion
    }
}
