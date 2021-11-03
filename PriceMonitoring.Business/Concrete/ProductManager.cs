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
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(Product product)
        {
           await _unitOfWork.Products.AddAsync(entity: product);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(Product product)
        {
            await _unitOfWork.Products.DeleteAsync(entity: product);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Product> Get(Expression<Func<Product, bool>> filter)
        {
            return await _unitOfWork.Products.GetAsync(filter: filter);
        }

        public async Task<IQueryable<Product>> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            return await _unitOfWork.Products.GetAllAsync(filter: filter);
        }

        public async Task Update(Product product)
        {
            await _unitOfWork.Products.UpdateAsync(entity: product);
            await _unitOfWork.SaveAsync();
        }
    }
}
