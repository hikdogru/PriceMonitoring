using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Entities.DTOs
{
    public class ProductWithPriceAndWebsiteDto
    {
        public Product Product { get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }
        public Website Website { get; set; }

    }
}
