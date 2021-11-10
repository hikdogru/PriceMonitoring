using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Business.Constants;
using PriceMonitoring.Core.Utilities.Results;
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
        public async Task<IResult> AddAsync(ProductPrice productPrice)
        {
            await _unitOfWork.ProductPrices.AddAsync(entity: productPrice);
            await _unitOfWork.SaveAsync();
            return new SuccessResult(message: Messages.ProductPricesAdded);
        }

        public async Task<IResult> DeleteAsync(ProductPrice productPrice)
        {
            await _unitOfWork.ProductPrices.DeleteAsync(entity: productPrice);
            await _unitOfWork.SaveAsync();
            return new SuccessResult(message: Messages.ProductPricesUpdated);
        }

        public async Task<IDataResult<ProductPrice>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<ProductPrice>(await _unitOfWork.ProductPrices.GetAsync(x => x.Id == id), message: Messages.ProductPriceListed);
        }

        public async Task<IDataResult<IQueryable<ProductPrice>>> GetAllAsync()
        {
            return new SuccessDataResult<IQueryable<ProductPrice>>(await _unitOfWork.ProductPrices.GetAllAsync(), message: Messages.ProductPricesListed);
        }

        public async Task<IDataResult<ProductPrice>> GetByProductIdAsync(int id)
        {
            return new SuccessDataResult<ProductPrice>(await _unitOfWork.ProductPrices.GetAsync(x => x.ProductId == id), message: Messages.ProductPriceListed);

        }

        public async Task<IResult> UpdateAsync(ProductPrice productPrice)
        {
            await _unitOfWork.ProductPrices.UpdateAsync(entity: productPrice);
            await _unitOfWork.SaveAsync();
            return new SuccessResult(message: Messages.ProductPricesUpdated);
        }

        public IDataResult<IQueryable<ProductPrice>> GetAll()
        {
            return new SuccessDataResult<IQueryable<ProductPrice>>(_unitOfWork.ProductPrices.GetAll(), message: Messages.ProductPricesListed);
        }

        public IDataResult<ProductPrice> GetById(int id)
        {
            return new SuccessDataResult<ProductPrice>(_unitOfWork.ProductPrices.Get(x => x.Id == id), message: Messages.ProductPriceListed);
        }

        public IDataResult<ProductPrice> GetByProductId(int id)
        {
            return new SuccessDataResult<ProductPrice>(_unitOfWork.ProductPrices.Get(x => x.ProductId == id), message: Messages.ProductPriceListed);
        }

        public IResult Add(ProductPrice productPrice)
        {
            _unitOfWork.ProductPrices.Add(entity: productPrice);
            _unitOfWork.Save();
            return new SuccessResult(message: Messages.ProductPricesAdded);
        }

        public IResult Update(ProductPrice productPrice)
        {
            _unitOfWork.ProductPrices.Update(entity: productPrice);
            _unitOfWork.Save();
            return new SuccessResult(message: Messages.ProductPricesUpdated);
        }

        public IResult Delete(ProductPrice productPrice)
        {
            _unitOfWork.ProductPrices.Delete(entity: productPrice);
            _unitOfWork.Save();
            return new SuccessResult(message: Messages.ProductPricesUpdated);
        }

        public IDataResult<IQueryable<ProductPrice>> GetProductsWithPriceAndWebsite()
        {
            return new SuccessDataResult<IQueryable<ProductPrice>>(_unitOfWork.ProductPrices.GetProductsWithPriceAndWebsite(), message: Messages.ProductPricesListed);
        }
    }
}
