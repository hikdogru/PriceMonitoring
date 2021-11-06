using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PriceMonitoring.Business.Abstract;
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Detail(int id)
        {
            var product = _productService.GetProductWithPriceById(id : id).Data;
            var productPriceModel = _mapper.Map<ProductPriceViewModel>(product);
            var prices = new List<double>();
            var dates = new List<string>();
            productPriceModel.ProductPrice.ToList().ForEach(x => prices.Add(x.Price));
            productPriceModel.ProductPrice.ToList().ForEach(x => dates.Add(x.SavedDate.ToString("dd,MM,yyyy")));
            ViewData["Prices"] = JsonConvert.SerializeObject(prices);
            ViewData["Dates"] = JsonConvert.SerializeObject(dates);
            return View(model: productPriceModel);
            
            
        }
    }
}
