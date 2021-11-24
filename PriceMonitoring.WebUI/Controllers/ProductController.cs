using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly IUserService _userService;
        private readonly IProductSubscriptionService _productSubscriptionService;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public ProductController(IProductService productService,
            IMapper mapper, IProductPriceService productPriceService,
            IUserService userService, IProductSubscriptionService productSubscriptionService)
        {
            _productService = productService;
            _mapper = mapper;
            _productPriceService = productPriceService;
            _userService = userService;
            _productSubscriptionService = productSubscriptionService;
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
            var products = new List<ChartJsonModel>();
            products.Add(new ChartJsonModel { Data = prices, Name = product.Name });
            productPriceModel.ProductPrice.ToList().ForEach(x => prices.Add(x.Price));
            productPriceModel.ProductPrice.ToList().ForEach(x => dates.Add(x.SavedDate.ToString("dd,MM,yyyy")));
            ViewData["Prices"] = JsonModel.SerializeObject(value: prices);
            ViewData["Dates"] = JsonModel.SerializeObject(value: dates);
            ViewData["Products"] = JsonModel.SerializeObject(value: products);
            return View(model: productPriceModel);
        }

        [HttpGet]
        public IActionResult Compare()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string q)
        {
            _chartProducts.Clear();
            if (!string.IsNullOrEmpty(q))
            {
                var result = _productService.Search(q: q);
                if (result.Success)
                {
                    _searchResults = new();
                    var products = result.Data.ToList();
                    products.ForEach(x => _searchResults.Add(_mapper.Map<ProductWithPriceAndWebsiteViewModel>(x)));
                    return View("Compare", model: _searchResults);
                }
            }
            return View("Compare");
        }


        [HttpGet]
        public IActionResult AddToCompare(int id)
        {
            var dates = _productPriceService.GetAll().Data.GroupBy(x => x.SavedDate.Date).Select(x => x.Key.ToShortDateString()).ToList();
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
                isExistProductInList = _chartProducts.Where(x => x.Name == product.Name).Count() > 0;
            }
            if (isExistProductInList == false)
            {
                _chartProducts.Add(new ChartJsonModel { Name = product.Name, Data = prices });
            }

            ViewData["Prices"] = JsonModel.SerializeObject(value: prices);
            ViewData["Dates"] = JsonModel.SerializeObject(value: _dates.OrderBy(x => x));
            ViewData["Products"] = JsonModel.SerializeObject(value: _chartProducts);

            return View("Compare", model: _searchResults.ToList());
        }


        [HttpPost]
        public IActionResult SubscribeToProduct(int id)
        {
            var product = _productService.GetProductWithPriceById(id);
            var user = _userService.GetByEmail(HttpContext.Session.GetString("email"));
            var result = _productSubscriptionService.Add(new ProductSubscription
            {
                ProductId = id,
                ProductPriceId = product.Data.ProductPrice.LastOrDefault().Id,
                UserId = user.Data.Id
            });
            if (result.Success)
            {
                TempData["AlertType"] = "success";
            }
            else
            {
                TempData["AlertType"] = "danger";
            }

            TempData["Message"] = result.Message;
            TempData["ProductId"] = id;

            return View(nameof(Compare), model: _searchResults);
        }

        [HttpGet("Subscriptions")]
        public IActionResult GetAllSubscriptions()
        {
            string email = HttpContext.Session.GetString("email");
            var user = _userService.GetByEmail(email: email);
            if (user.Success)
            {
                var products = new List<ProductWithPriceAndWebsiteViewModel>();
                var productSubcriptions = _productSubscriptionService.GetAllByUserId(userId: user.Data.Id).Data.ToList();

                foreach (var sub in productSubcriptions)
                {
                    var product = _productService.GetProductsWithPriceAndWebsite().Data.Where(x => x.Product.Id == sub.ProductId).SingleOrDefault();
                    products.Add(_mapper.Map<ProductWithPriceAndWebsiteViewModel>(product));
                }
                return View(viewName: "Subscriptions", model: products);

            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteSubscription(int id)
        {
            var user = _userService.GetByEmail(email: HttpContext.Session.GetString("email"));
            var productSubcriptions = _productSubscriptionService.GetAllByUserId(userId: user.Data.Id).Data.ToList();
            var productSubscription = productSubcriptions.Where(x => x.ProductId == id).SingleOrDefault();
            var result = _productSubscriptionService.Delete(productSubscription);

            if (result.Success)
            {
                TempData["AlertType"] = "success";
            }
            else
            {
                TempData["AlertType"] = "danger";
            }

            TempData["Message"] = result.Message;
            TempData["ProductId"] = id;
            return RedirectToAction(nameof(GetAllSubscriptions));
        }
        #endregion
    }
}
