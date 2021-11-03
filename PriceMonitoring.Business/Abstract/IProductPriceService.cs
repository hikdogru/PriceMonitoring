using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.Abstract
{
    public interface IProductPriceService
    {
        Task<IQueryable<ProductPrice>> GetAll(Expression<Func<ProductPrice, bool>> filter = null);
        Task<ProductPrice> Get(Expression<Func<ProductPrice, bool>> filter);
        Task Add(ProductPrice productPrice);
        Task Update(ProductPrice productPrice);
        Task Delete(ProductPrice productPrice);
    }
}
