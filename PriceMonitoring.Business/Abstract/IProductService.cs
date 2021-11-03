using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.Abstract
{
    public interface IProductService
    {
        Task<IQueryable<Product>> GetAll(Expression<Func<Product, bool>> filter = null);
        Task<Product> Get(Expression<Func<Product, bool>> filter);
        Task Add(Product product);
        Task Update(Product product);
        Task Delete(Product product);
    }
}
