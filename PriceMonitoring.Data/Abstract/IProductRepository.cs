using PriceMonitoring.Core.Data;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Abstract
{
    public interface IProductRepository : IEntityRepository<Product>
    {
        IQueryable<Product> GetProductsWithPrice(Expression<Func<Product, bool>> filter = null);
        IQueryable<Product> Search(string q);

    }
}
