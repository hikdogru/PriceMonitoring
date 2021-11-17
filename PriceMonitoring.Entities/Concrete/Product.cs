using PriceMonitoring.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Entities.Concrete
{
    public class Product : IEntity
    {
        private string _name;

        public int Id { get; set; }
        public string Name
        {
            get
            {
                return _name.Length > 25 ? _name.Substring(0, 21) + "..." : _name;
            }
            set
            {
                _name = value;
            }
        }


        public string Image { get; set; }
        public int WebsiteId { get; set; }
        public Website Website { get; set; }
        public ICollection<ProductPrice> ProductPrice { get; set; }

    }
}
