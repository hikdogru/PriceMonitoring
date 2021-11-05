using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Entities.DTOs
{
    public class ProductWithPriceDto
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
    }
}
