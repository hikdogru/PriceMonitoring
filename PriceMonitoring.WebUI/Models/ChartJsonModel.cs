using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.Models
{
    public class ChartJsonModel
    {
        public string Name { get; set; }
        public List<double> Data { get; set; }
    }
}
