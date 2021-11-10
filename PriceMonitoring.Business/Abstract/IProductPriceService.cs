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
    public interface IProductPriceService
    {
        #region async
        Task<IDataResult<IQueryable<ProductPrice>>> GetAllAsync();
        Task<IDataResult<ProductPrice>> GetByIdAsync(int id);
        Task<IDataResult<ProductPrice>> GetByProductIdAsync(int id);
        Task<IResult> AddAsync(ProductPrice productPrice);
        Task<IResult> UpdateAsync(ProductPrice productPrice);
        Task<IResult> DeleteAsync(ProductPrice productPrice);
        #endregion

        #region sync
        IDataResult<IQueryable<ProductPrice>> GetAll();
        IDataResult<ProductPrice> GetById(int id);
        IDataResult<ProductPrice> GetByProductId(int id);
        IResult Add(ProductPrice productPrice);
        IResult Update(ProductPrice productPrice);
        IResult Delete(ProductPrice productPrice);
        IDataResult<IQueryable<ProductPrice>> GetProductsWithPriceAndWebsite();
        #endregion
    }
}
