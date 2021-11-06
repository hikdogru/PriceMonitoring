using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.Models
{
    public class ProductPriceModel
    {
        public int ProductId { get; set; }
        public double Price { get; set; }
        public ProductModel Product { get; set; }
        public DateTime SavedDate { get; set; }
    }
}
