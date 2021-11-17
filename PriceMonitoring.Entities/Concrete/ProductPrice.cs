using PriceMonitoring.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Entities.Concrete
{
    public class ProductPrice : IEntity
    {
        private DateTime _date;

        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double Price { get; set; }
        public DateTime SavedDate
        {
            get
            {
                return _date.Date;
            }

            set
            {
                _date = DateTime.Parse(value.ToString(), new CultureInfo("en-US"));
            }

        }



    }
}
