using PriceMonitoring.Core.Utilities.Results;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.Abstract
{
    public interface IProductSubscriptionService
    {
        #region async
        Task<IDataResult<IQueryable<ProductSubscription>>> GetAllAsync();
        Task<IDataResult<ProductSubscription>> GetByIdAsync(int id);
        Task<IResult> AddAsync(ProductSubscription productSubscription);
        Task<IResult> UpdateAsync(ProductSubscription productSubscription);
        Task<IResult> DeleteAsync(ProductSubscription productSubscription);
        #endregion

        #region sync
        IDataResult<IQueryable<ProductSubscription>> GetAll();
        IDataResult<ProductSubscription> GetById(int id);
        IResult Add(ProductSubscription productSubscription);
        IResult Update(ProductSubscription productSubscription);
        IResult Delete(ProductSubscription productSubscription);
        ValidationResult IsProductSubscriptionValidate(ProductSubscription productSubscription);
        #endregion
    }
}
