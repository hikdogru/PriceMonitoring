using FluentValidation.Results;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Business.Constants;
using PriceMonitoring.Business.ValidationRules.FluentValidation;
using PriceMonitoring.Core.CrossCuttingConcerns.FluentValidation;
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
        #region fields

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region ctor

        public ProductPriceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region methods
        public async Task<IResult> AddAsync(ProductPrice productPrice)
        {
            var sb = new StringBuilder();
            var isProductPriceExist = IsProductPriceExistInDatabase(productPrice: productPrice);
            var validationResult = IsProductPriceValid(productPrice: productPrice);
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            if (isProductPriceExist == false)
            {
                await _unitOfWork.ProductPrices.AddAsync(entity: productPrice);
                await _unitOfWork.SaveAsync();
                return new SuccessResult(message: Messages.ProductPricesAdded);
            }

            return new ErrorResult(message: Messages.ProductPriceIsExist);
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
            var isProductPriceExist = GetById(id: productPrice.Id).Success;
            var validationResult = IsProductPriceValid(productPrice: productPrice);
            var sb = new StringBuilder();
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            if (isProductPriceExist)
            {
                await _unitOfWork.ProductPrices.UpdateAsync(entity: productPrice);
                await _unitOfWork.SaveAsync();
                return new SuccessResult(message: Messages.ProductPricesUpdated);
            }


            return new ErrorResult(message: Messages.ProductSearchNotExist);

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
            var sb = new StringBuilder();
            var isProductPriceExist = IsProductPriceExistInDatabase(productPrice: productPrice);
            var validationResult = IsProductPriceValid(productPrice: productPrice);
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            if (isProductPriceExist == false)
            {
                _unitOfWork.ProductPrices.Add(entity: productPrice);
                _unitOfWork.Save();
                return new SuccessResult(message: Messages.ProductPricesAdded);
            }

            return new ErrorResult(message: Messages.ProductPriceIsExist);

        }

        public IResult Update(ProductPrice productPrice)
        {
            var isProductPriceExist = GetById(id: productPrice.Id).Success;
            var validationResult = IsProductPriceValid(productPrice: productPrice);
            var sb = new StringBuilder();
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            if (isProductPriceExist)
            {
                _unitOfWork.ProductPrices.Update(entity: productPrice);
                _unitOfWork.Save();
                return new SuccessResult(message: Messages.ProductPricesUpdated);
            }


            return new ErrorResult(message: Messages.ProductSearchNotExist);
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

        public bool IsProductPriceExistInDatabase(ProductPrice productPrice)
        {
            return GetAll().Data.Where(x => x.ProductId == productPrice.ProductId &&
                                            x.SavedDate.Date == productPrice.SavedDate.Date).Count() > 0;
        }

        public ValidationResult IsProductPriceValid(ProductPrice productPrice)
        {
            return ValidationTool.Validate(new ProductPriceValidator(), productPrice);
        }

        #endregion
    }
}
