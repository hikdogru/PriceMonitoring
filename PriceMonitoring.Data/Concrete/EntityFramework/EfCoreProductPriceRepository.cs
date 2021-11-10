using Microsoft.EntityFrameworkCore;
using PriceMonitoring.Core.Data.EntityFramework;
using PriceMonitoring.Data.Abstract;
using PriceMonitoring.Data.Concrete.EntityFramework.Contexts;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Concrete.EntityFramework
{
    public class EfCoreProductPriceRepository : EfEntityRepositoryBase<ProductPrice, PriceMonitoringContext>, IProductPriceRepository
    {
        private readonly PriceMonitoringContext _context;

        public EfCoreProductPriceRepository(PriceMonitoringContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<ProductPrice> GetProductsWithPriceAndWebsite(Expression<Func<ProductPrice, bool>> filter = null)
        {
            var products = _context.ProductPrices.Include(x => x.Product).Include(x => x.Product.Website);

            return filter == null ? products : products.Where(filter);
        }
    }
}
