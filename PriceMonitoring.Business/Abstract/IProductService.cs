using FluentValidation.Results;
using PriceMonitoring.Core.Utilities.Results;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
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
        IDataResult<IQueryable<ProductWithPriceAndWebsiteDto>> GetProductsWithPriceAndWebsite(Expression<Func<Product, bool>> filter = null);
        IDataResult<Product> GetProductWithPriceById(int id);
        IDataResult<IQueryable<ProductWithPriceAndWebsiteDto>> Search(string q);
        ValidationResult IsProductValidate(Product product);
        IDataResult<IQueryable<ProductListDto>> GetProductListDto();
        #endregion
    }
}
