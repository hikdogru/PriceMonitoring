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
        #region sync
        IQueryable<ProductListDto> GetProductListDto(Expression<Func<ProductListDto, bool>> filter = null);
        IQueryable<Product> GetProductsWithPrice(Expression<Func<Product, bool>> filter = null);
        IQueryable<ProductWithPriceAndWebsiteDto> GetProductsWithPriceAndWebsite(Expression<Func<ProductWithPriceAndWebsiteDto, bool>> filter = null);
        IQueryable<ProductWithPriceAndWebsiteDto> Search(string q);
        #endregion

        #region async

        Task<IQueryable<ProductListDto>> GetProductListDtoAsync(Expression<Func<ProductListDto, bool>> filter = null);
        IQueryable<ProductListDto> GetProductListDtoAsSqlView(Expression<Func<ProductListDto, bool>> filter = null);

        #endregion

    }
}
