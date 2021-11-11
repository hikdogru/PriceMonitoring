using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Entities.DTOs
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public int WebsiteId { get; set; }
    }
}
