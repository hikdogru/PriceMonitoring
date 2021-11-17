using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Business.Constants;
using PriceMonitoring.Core.Utilities.Results;
using PriceMonitoring.Data.Abstract;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.Concrete
{
    public class ProductSubscriptionManager : IProductSubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductSubscriptionManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IResult Add(ProductSubscription productSubscription)
        {
            _unitOfWork.ProductSubscriptions.Add(productSubscription);
            _unitOfWork.Save();
            return new SuccessResult(message: Messages.ProductSubscriptionAdded);
        }

        public Task<IResult> AddAsync(ProductSubscription productSubscription)
        {
            throw new NotImplementedException();
        }

        public IResult Delete(ProductSubscription productSubscription)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(ProductSubscription productSubscription)
        {
            throw new NotImplementedException();
        }

        public IDataResult<IQueryable<ProductSubscription>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IQueryable<ProductSubscription>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IDataResult<ProductSubscription> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<ProductSubscription>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public ValidationResult IsProductSubscriptionValidate(ProductSubscription productSubscription)
        {
            throw new NotImplementedException();
        }

        public IResult Update(ProductSubscription productSubscription)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(ProductSubscription productSubscription)
        {
            throw new NotImplementedException();
        }
    }
}
