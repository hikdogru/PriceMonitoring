﻿using PriceMonitoring.Core.Data.EntityFramework;
using PriceMonitoring.Data.Abstract;
using PriceMonitoring.Data.Concrete.EntityFramework.Contexts;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Concrete.EntityFramework
{
    public class EfCoreProductPriceRepository : EfEntityRepositoryBase<ProductPrice, PriceMonitoringContext>, IProductPriceRepository
    {
        public EfCoreProductPriceRepository(PriceMonitoringContext context) : base(context)
        {
        }
    }
}
