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
        private static List<ProductWithPriceAndWebsiteViewModel> _searchResults;
        private static List<ChartJsonModel> _chartProducts = new();
        private static List<string> _dates = new();
        private readonly IProductService _productService;
        private readonly IProductPriceService _productPriceService;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public ProductController(IProductService productService, IMapper mapper, IProductPriceService productPriceService)
        {
            _productService = productService;
            _mapper = mapper;
            _productPriceService = productPriceService;
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
                    result.Data.ToList().ForEach(x => _searchResults.Add(_mapper.Map<ProductWithPriceAndWebsiteViewModel>(x)));
                    return View("Compare", model: _searchResults);
                }
            }
            return View("Compare");
        }


        [HttpPost]
        public IActionResult AddToCompare(int id)
        {
            List<string> dates = new() { "11/4/2021", "11/5/2021", "11/6/2021", "11/7/2021", "11/8/2021", "11/9/2021", "11/10/2021" };
           
            var product = _productService.GetProductWithPriceById(id: id).Data;
            if (dates.Count > product.ProductPrice.Count)
            {
                List<string> dates2 = new();
                var productList = _productService.GetProductWithPriceById(product.Id).Data;

                foreach (var item in productList.ProductPrice)
                {
                    dates2.Add(item.SavedDate.Date.ToString("MM/d/yyyy"));
                }

                var diff = dates.Except(dates2).ToList();
                foreach (var item in diff)
                {
                    var productPrice = new ProductPrice
                    {
                        SavedDate = DateTime.Parse(item),
                        Price = 0,
                        ProductId = product.Id
                    };
                    _productPriceService.Add(productPrice: productPrice);
                }
            }
            var productPriceModel = _mapper.Map<ProductPriceViewModel>(product);
            var prices = new List<double>();

            productPriceModel.ProductPrice.OrderBy(x => x.SavedDate).ToList().ForEach(x => prices.Add(x.Price));

            if (_dates.Count == 0 || _dates.Count < prices.Count)
            {
                productPriceModel.ProductPrice.OrderBy(x => x.SavedDate).ToList().ForEach(x => _dates.Add(x.SavedDate.ToString("dd,MM,yyyy")));
            }

            var isExistProductInList = false;
            if (_chartProducts.Count > 0)
            {
                isExistProductInList = _chartProducts.Select(x => x.Name == product.Name).First();
            }
            if (isExistProductInList == false)
            {
                _chartProducts.Add(new ChartJsonModel { Name = product.Name, Data = prices });
            }
            ViewData["Prices"] = JsonConvert.SerializeObject(prices);
            ViewData["Dates"] = JsonConvert.SerializeObject(_dates.OrderBy(x => x));
            ViewData["Products"] = JsonConvert.SerializeObject(_chartProducts, Formatting.Indented,
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                                                                        ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
                                                                    });
            
            return View("Compare", model: _searchResults.ToList());
        }

        #endregion
    }
}
