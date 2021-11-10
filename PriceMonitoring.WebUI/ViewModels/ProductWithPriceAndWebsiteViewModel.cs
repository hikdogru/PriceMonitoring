using PriceMonitoring.Entities.Concrete;
using System.Collections.Generic;

namespace PriceMonitoring.WebUI.ViewModels
{
    public class ProductWithPriceAndWebsiteViewModel
    {
        public Product Product { get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }
        public Website Website { get; set; }
    }
}
