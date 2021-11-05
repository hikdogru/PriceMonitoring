using PriceMonitoring.Core.Utilities.Results;
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
        #region async
        Task<IDataResult<IQueryable<Product>>> GetAllAsync();
        Task<IDataResult<Product>> GetByIdAsync(int id);
        Task<IDataResult<Product>> GetByImageSourceAsync(string imgSource);
        Task<IResult> AddAsync(Product product);
        Task<IResult> UpdateAsync(Product product);
        Task<IResult> DeleteAsync(Product product);
        #endregion

        #region sync
        IDataResult<IQueryable<Product>> GetAll();
        IDataResult<Product> GetById(int id);
        IDataResult<Product> GetByImageSource(string imgSource);
        IResult Add(Product product);
        IResult Update(Product product);
        IResult Delete(Product product);
        IDataResult<IQueryable<Product>> GetProductsWithPrice();
        #endregion
    }
}
