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
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.Concrete
{
    public class ProductSubscriptionManager : IProductSubscriptionService
    {
        #region fields

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region ctor
        public ProductSubscriptionManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region methods

        public IResult Add(ProductSubscription productSubscription)
        {
            var isProductSubscriptionExist = IsProductSubscriptionExistInDatabase(productSubscription: productSubscription);
            var validationResult = IsProductSubscriptionValid(productSubscription: productSubscription);
            var sb = new StringBuilder();

            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            if (!isProductSubscriptionExist)
            {
                _unitOfWork.ProductSubscriptions.Add(productSubscription);
                _unitOfWork.Save();
                return new SuccessResult(message: Messages.ProductSubscriptionAdded);
            }

            return new ErrorResult(message: Messages.ProductSubscriptionIsExist);

        }

        public async Task<IResult> AddAsync(ProductSubscription productSubscription)
        {
            var isProductSubscriptionExist = IsProductSubscriptionExistInDatabase(productSubscription: productSubscription);
            var validationResult = IsProductSubscriptionValid(productSubscription: productSubscription);
            var sb = new StringBuilder();

            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            if (!isProductSubscriptionExist)
            {
                await _unitOfWork.ProductSubscriptions.AddAsync(productSubscription);
                await _unitOfWork.SaveAsync();
                return new SuccessResult(message: Messages.ProductSubscriptionAdded);
            }

            return new ErrorResult(message: Messages.ProductSubscriptionIsExist);
        }

        public IResult Delete(ProductSubscription productSubscription)
        {
            var isProductSubscriptionExist = GetById(id: productSubscription.Id);
            if (isProductSubscriptionExist.Success)
            {
                _unitOfWork.ProductSubscriptions.Delete(productSubscription);
                _unitOfWork.Save();
                return new SuccessResult(message: Messages.ProductSubscriptionDeleted);
            }

            return new ErrorResult(message: Messages.ProductSubscriptionSearchNotExist);

        }

        public async Task<IResult> DeleteAsync(ProductSubscription productSubscription)
        {
            var isProductSubscriptionExist = GetById(id: productSubscription.Id);
            if (isProductSubscriptionExist.Success)
            {
                await _unitOfWork.ProductSubscriptions.DeleteAsync(productSubscription);
                await _unitOfWork.SaveAsync();
                return new SuccessResult(message: Messages.ProductSubscriptionDeleted);
            }

            return new ErrorResult(message: Messages.ProductSubscriptionSearchNotExist);
        }

        public IDataResult<IQueryable<ProductSubscription>> GetAll()
        {
            return new SuccessDataResult<IQueryable<ProductSubscription>>(data: _unitOfWork.ProductSubscriptions.GetAll(), message: Messages.ProductSubscriptionsListed);
        }

        public async Task<IDataResult<IQueryable<ProductSubscription>>> GetAllAsync()
        {
            return new SuccessDataResult<IQueryable<ProductSubscription>>(data: await _unitOfWork.ProductSubscriptions.GetAllAsync(), message: Messages.ProductSubscriptionsListed);
        }

        public IDataResult<ProductSubscription> GetById(int id)
        {
            var entity = _unitOfWork.ProductSubscriptions.Get(x => x.Id == id);
            if (entity == null)
            {
                return new ErrorDataResult<ProductSubscription>(message: Messages.ProductSubscriptionSearchNotExist);
            }
            return new SuccessDataResult<ProductSubscription>(data: entity, Messages.ProductSubscriptionListed);
        }

        public async Task<IDataResult<ProductSubscription>> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.ProductSubscriptions.GetAsync(x => x.Id == id);
            if (entity == null)
            {
                return new ErrorDataResult<ProductSubscription>(message: Messages.ProductSubscriptionSearchNotExist);
            }
            return new SuccessDataResult<ProductSubscription>(data: entity, Messages.ProductSubscriptionListed);
        }

        public ValidationResult IsProductSubscriptionValid(ProductSubscription productSubscription)
        {
            return ValidationTool.Validate(validator: new ProductSubscriptionValidator(), entity: productSubscription);
        }

        private bool IsProductSubscriptionExistInDatabase(ProductSubscription productSubscription)
        {
            return _unitOfWork.ProductSubscriptions.GetAll(x => x.UserId == productSubscription.UserId &&
                                                                x.ProductId == productSubscription.ProductId &&
                                                                x.ProductPriceId == productSubscription.ProductPriceId).Count() > 0;
        }

        public IResult Update(ProductSubscription productSubscription)
        {
            var isProductSubscriptionExist = IsProductSubscriptionExistInDatabase(productSubscription: productSubscription);
            var validationResult = IsProductSubscriptionValid(productSubscription: productSubscription);
            var sb = new StringBuilder();

            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            if (isProductSubscriptionExist)
            {
                _unitOfWork.ProductSubscriptions.Update(productSubscription);
                _unitOfWork.Save();
                return new SuccessResult(message: Messages.ProductSubscriptionUpdated);
            }

            return new SuccessResult(message: Messages.ProductSubscriptionSearchNotExist);
        }

        public async Task<IResult> UpdateAsync(ProductSubscription productSubscription)
        {

            var isProductSubscriptionExist = IsProductSubscriptionExistInDatabase(productSubscription: productSubscription);
            var validationResult = IsProductSubscriptionValid(productSubscription: productSubscription);
            var sb = new StringBuilder();

            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            if (isProductSubscriptionExist)
            {
                await _unitOfWork.ProductSubscriptions.UpdateAsync(productSubscription);
                await _unitOfWork.SaveAsync();
                return new SuccessResult(message: Messages.ProductSubscriptionUpdated);

            }
            return new SuccessResult(message: Messages.ProductSubscriptionSearchNotExist);
        }

        public async Task<IDataResult<IQueryable<ProductSubscription>>> GetAllByUserIdAsync(int userId)
        {
            var subscriptions = await _unitOfWork.ProductSubscriptions.GetAllAsync(x => x.UserId == userId);
            if (subscriptions == null)
            {
                return new ErrorDataResult<IQueryable<ProductSubscription>>(message: Messages.ProductSubscriptionSearchNotExist);
            }
            return new SuccessDataResult<IQueryable<ProductSubscription>>(data: subscriptions, message: Messages.ProductSubscriptionsListed);
        }

        public IDataResult<IQueryable<ProductSubscription>> GetAllByUserId(int userId)
        {
            var subscriptions = _unitOfWork.ProductSubscriptions.GetAll(x => x.UserId == userId);
            if (subscriptions == null)
            {
                return new ErrorDataResult<IQueryable<ProductSubscription>>(message: Messages.ProductSubscriptionSearchNotExist);
            }
            return new SuccessDataResult<IQueryable<ProductSubscription>>(data: subscriptions, message: Messages.ProductSubscriptionsListed);
        }

        #endregion
    }
}
