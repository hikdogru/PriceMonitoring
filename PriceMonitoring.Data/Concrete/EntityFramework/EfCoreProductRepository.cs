using Microsoft.EntityFrameworkCore;
using PriceMonitoring.Core.Data.EntityFramework;
using PriceMonitoring.Data.Abstract;
using PriceMonitoring.Data.Concrete.EntityFramework.Contexts;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Concrete.EntityFramework
{
    public class EfCoreProductRepository : EfEntityRepositoryBase<Product, PriceMonitoringContext>, IProductRepository
    {
        private PriceMonitoringContext _context;
        public EfCoreProductRepository(PriceMonitoringContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Product> GetProductsWithPrice(Expression<Func<Product, bool>> filter = null)
        {
            var products = _context.Products.Include(x => x.ProductPrice);
            
            return filter == null ? products : products.Where(filter);
        }

        public IQueryable<Product> Search(string q)
        {
            var products = _context.Products.Where(x => x.Name.Contains(q));
            return products;
        }
    }
}
