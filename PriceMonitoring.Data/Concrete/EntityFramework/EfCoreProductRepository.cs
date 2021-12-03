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
        #region fields

        private PriceMonitoringContext _context;

        #endregion

        #region ctor

        public EfCoreProductRepository(PriceMonitoringContext context) : base(context)
        {
            _context = context;
        }

        #endregion

        #region methods

        public IQueryable<Product> GetProductsWithPrice(Expression<Func<Product, bool>> filter = null)
        {
            var products = _context.Products.Include(x => x.ProductPrice).AsSplitQuery();

            return filter == null ? products : products.Where(filter);
        }

        public IQueryable<ProductWithPriceAndWebsiteDto> GetProductsWithPriceAndWebsite(Expression<Func<ProductWithPriceAndWebsiteDto, bool>> filter = null)
        {
            var products = from product in GetProductsWithPrice()
                           join website in _context.Websites on product.WebsiteId equals website.Id
                           select new ProductWithPriceAndWebsiteDto()
                           {
                               Product = product,
                               Website = website,
                               ProductPrices = product.ProductPrice
                           };

            return products;
        }

        public IQueryable<ProductListDto> GetProductListDto(Expression<Func<ProductListDto, bool>> filter = null)
        {
            var products = from product in _context.Products
                           select new ProductListDto()
                           {
                               Id = product.Id,
                               Name = product.Name,
                               Image = product.Image,
                               WebsiteId = product.WebsiteId
                           };
            return filter == null ? products : products.Where(filter);
        }

        public IQueryable<ProductListDto> GetProductListDtoAsSqlView(Expression<Func<ProductListDto, bool>> filter = null)
        {
            var products = _context.ProductList_View.AsQueryable();
            return filter == null ? products : products.Where(filter);
        }

        public IQueryable<ProductWithPriceAndWebsiteDto> Search(string q)
        {
            var products = GetProductsWithPriceAndWebsite().Where(x => x.Product.Name.Contains(q)).AsQueryable();
            return products;
        }

        public async Task<IQueryable<ProductListDto>> GetProductListDtoAsync(Expression<Func<ProductListDto, bool>> filter = null)
        {
            var products = (from product in await _context.Products.ToListAsync()
                           select new ProductListDto()
                           {
                               Id = product.Id,
                               Name = product.Name,
                               Image = product.Image,
                               WebsiteId = product.WebsiteId
                           }).AsQueryable();
            return filter == null ? products : products.Where(filter);
        }
    }

    #endregion
}
