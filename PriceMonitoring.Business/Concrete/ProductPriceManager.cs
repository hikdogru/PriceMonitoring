using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Data.Abstract;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.Concrete
{
    public class ProductPriceManager : IProductPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductPriceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(ProductPrice productPrice)
        {
            await _unitOfWork.ProductPrices.AddAsync(entity: productPrice);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(ProductPrice productPrice)
        {
            await _unitOfWork.ProductPrices.DeleteAsync(entity: productPrice);
            await _unitOfWork.SaveAsync();
        }

        public async Task<ProductPrice> Get(Expression<Func<ProductPrice, bool>> filter)
        {
            return await _unitOfWork.ProductPrices.GetAsync(filter: filter);
        }

        public async Task<IQueryable<ProductPrice>> GetAll(Expression<Func<ProductPrice, bool>> filter = null)
        {
            return await _unitOfWork.ProductPrices.GetAllAsync(filter: filter);
        }

        public async Task Update(ProductPrice productPrice)
        {
            await _unitOfWork.ProductPrices.UpdateAsync(entity: productPrice);
            await _unitOfWork.SaveAsync();
        }


    }
}
