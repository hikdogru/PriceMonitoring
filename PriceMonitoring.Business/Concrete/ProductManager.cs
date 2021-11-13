using FluentValidation.Results;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Business.Constants;
using PriceMonitoring.Business.ValidationRules.FluentValidation;
using PriceMonitoring.Core.CrossCuttingConcerns.FluentValidation;
using PriceMonitoring.Core.Utilities.Results;
using PriceMonitoring.Data.Abstract;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
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
        #region fields

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region ctor

        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region methods
        public IResult Add(Product product)
        {
            var sb = new StringBuilder();
            var isProductExist = IsProductExistInDatabase(product: product);
            var validationResult = IsProductValidate(product: product);
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            if (isProductExist == false)
            {
                _unitOfWork.Products.Add(entity: product);
                _unitOfWork.Save();
                return new SuccessResult(message: Messages.ProductAdded);
            }

            return new ErrorResult(message: Messages.ProductIsExist);

        }

        public async Task<IResult> AddAsync(Product product)
        {
            await _unitOfWork.Products.AddAsync(entity: product);
            await _unitOfWork.SaveAsync();
            return new SuccessResult(message: Messages.ProductAdded);
        }

        public IResult Delete(Product product)
        {
            var isExistProductInDatabase = GetById(id: product.Id);
            if (isExistProductInDatabase.Success)
            {
                _unitOfWork.Products.Delete(entity: isExistProductInDatabase.Data);
                _unitOfWork.Save();
                return new SuccessResult(message: Messages.ProductDeleted);
            }

            return new ErrorResult(message: Messages.ProductSearchNotExist);
        }

        public async Task<IResult> DeleteAsync(Product product)
        {
            await _unitOfWork.Products.DeleteAsync(entity: product);
            await _unitOfWork.SaveAsync();
            return new SuccessResult(message: Messages.ProductDeleted);
        }

        public IDataResult<IQueryable<Product>> GetAll()
        {
            return new SuccessDataResult<IQueryable<Product>>(_unitOfWork.Products.GetAll());
        }

        public async Task<IDataResult<IQueryable<Product>>> GetAllAsync()
        {
            return new SuccessDataResult<IQueryable<Product>>(await _unitOfWork.Products.GetAllAsync(), message: Messages.ProductsListed);
        }

        public IDataResult<Product> GetById(int id)
        {
            var product = _unitOfWork.Products.Get(x => x.Id == id);
            if (product == null)
            {
                return new ErrorDataResult<Product>(product, message: Messages.ProductSearchNotExist);
            }

            return new SuccessDataResult<Product>(product, message: Messages.ProductListed);
        }

        public async Task<IDataResult<Product>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<Product>(await _unitOfWork.Products.GetAsync(x => x.Id == id), message: Messages.ProductListed);
        }

        public IDataResult<Product> GetByImageSource(string imgSource)
        {
            return new SuccessDataResult<Product>(_unitOfWork.Products.GetAll(x => x.Image == imgSource).OrderBy(x => x.Id).LastOrDefault(), message: Messages.ProductListed);
        }

        public async Task<IDataResult<Product>> GetByImageSourceAsync(string imgSource)
        {
            return new SuccessDataResult<Product>(await _unitOfWork.Products.GetAsync(x => x.Image == imgSource), message: Messages.ProductListed);
        }

        public IDataResult<IQueryable<Product>> GetProductsWithPrice()
        {
            return new SuccessDataResult<IQueryable<Product>>(_unitOfWork.Products.GetProductsWithPrice(), message: Messages.ProductsListed);
        }

        public IDataResult<IQueryable<ProductWithPriceAndWebsiteDto>> GetProductsWithPriceAndWebsite(Expression<Func<Product, bool>> filter = null)
        {
            return new SuccessDataResult<IQueryable<ProductWithPriceAndWebsiteDto>>(_unitOfWork.Products.GetProductsWithPriceAndWebsite(), message: Messages.ProductsListed);
        }

        public IDataResult<Product> GetProductWithPriceById(int id)
        {
            return new SuccessDataResult<Product>(GetProductsWithPrice().Data.Where(x => x.Id == id).SingleOrDefault());
        }

        public IDataResult<IQueryable<ProductWithPriceAndWebsiteDto>> Search(string q)
        {
            var products = _unitOfWork.Products.Search(q);
            if (products.Count() == 0)
            {
                return new ErrorDataResult<IQueryable<ProductWithPriceAndWebsiteDto>>(message: Messages.ProductSearchNotExist);
            }

            return new SuccessDataResult<IQueryable<ProductWithPriceAndWebsiteDto>>(data: products, message: Messages.ProductsListed);
        }

        public IResult Update(Product product)
        {
            var isProductExist = GetById(id: product.Id).Success;
            var validationResult = IsProductValidate(product: product);
            var sb = new StringBuilder();
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            if (isProductExist)
            {
                _unitOfWork.Products.Update(entity: product);
                _unitOfWork.Save();
                return new SuccessResult(message: Messages.ProductUpdated);
            }

            return new ErrorResult(message: Messages.ProductPriceNotExist);

        }

        public async Task<IResult> UpdateAsync(Product product)
        {
            var isProductExist = await GetByIdAsync(id: product.Id);
            var validationResult = IsProductValidate(product: product);
            var sb = new StringBuilder();
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }
            if (isProductExist.Success)
            {
                await _unitOfWork.Products.UpdateAsync(entity: product);
                await _unitOfWork.SaveAsync();
                return new SuccessResult(message: Messages.ProductUpdated);
            }
            return new ErrorResult(message: Messages.ProductSearchNotExist);

        }

        private bool IsProductExistInDatabase(Product product)
        {
            return _unitOfWork.Products.GetAll(x => x.Image == product.Image).Count() > 0;
        }

        public ValidationResult IsProductValidate(Product product)
        {
            return ValidationTool.Validate(new ProductValidator(), product);
        }

        #endregion
    }
}
