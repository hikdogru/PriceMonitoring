using PriceMonitoring.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Entities.Concrete
{
    public class ProductSubscription : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int ProductPriceId { get; set; }

    }
}
