using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.ViewModels
{
    public class ProductPriceViewModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public ICollection<ProductPrice> ProductPrice { get; set; }

    }
}
