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
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IResult Add(Product product)
        {
            var isProductExist = IsProductExistInDatabase(product: product);
            if (isProductExist == false)
            {
                _unitOfWork.Products.Add(entity: product);
                _unitOfWork.Save();
                return new SuccessResult(message: Messages.ProductAdded);
            }

            return new ErrorResult(message: "Error");
            
        }

        public async Task<IResult> AddAsync(Product product)
        {
            await _unitOfWork.Products.AddAsync(entity: product);
            await _unitOfWork.SaveAsync();
            return new SuccessResult(message: Messages.ProductAdded);
        }

        public IResult Delete(Product product)
        {
            _unitOfWork.Products.Delete(entity: product);
            _unitOfWork.Save();
            return new SuccessResult(message: Messages.ProductDeleted);
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
            return new SuccessDataResult<Product>(_unitOfWork.Products.Get(x => x.Id == id), message: Messages.ProductListed);
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
            return new SuccessDataResult<IQueryable<Product>>(_unitOfWork.Products.GetProductsWithPrice(), message: Messages.ProductListed);
        }

        public IDataResult<Product> GetProductWithPriceById(int id)
        {
            return new SuccessDataResult<Product>(GetProductsWithPrice().Data.Where(x => x.Id == id).SingleOrDefault());
        }

        public IResult Update(Product product)
        {
            _unitOfWork.Products.Update(entity: product);
            _unitOfWork.Save();
            return new SuccessResult(message: Messages.ProductUpdated);
        }

        public async Task<IResult> UpdateAsync(Product product)
        {
            await _unitOfWork.Products.UpdateAsync(entity: product);
            await _unitOfWork.SaveAsync();
            return new SuccessResult(message: Messages.ProductUpdated);
        }

        private bool IsProductExistInDatabase(Product product)
        {
            return _unitOfWork.Products.GetAll(x => x.Name == product.Name && x.Image == product.Image).Count() > 0;
        }
    }
}
