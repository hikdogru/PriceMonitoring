﻿using FluentValidation.Results;
using PriceMonitoring.Core.Utilities.Results;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
using System;
using System.Collections.Generic;
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
        Task<IDataResult<IQueryable<ProductSubscription>>> GetAllByUserIdAsync(int userId);

        Task<IResult> AddAsync(ProductSubscription productSubscription);
        Task<IResult> UpdateAsync(ProductSubscription productSubscription);
        Task<IResult> DeleteAsync(ProductSubscription productSubscription);
        #endregion

        #region sync
        IDataResult<IQueryable<ProductSubscription>> GetAll();
        IDataResult<ProductSubscription> GetById(int id);
        IDataResult<IQueryable<ProductSubscription>> GetAllByUserId(int userId);
        IResult Add(ProductSubscription productSubscription);
        IResult Update(ProductSubscription productSubscription);
        IResult Delete(ProductSubscription productSubscription);
        ValidationResult IsProductSubscriptionValid(ProductSubscription productSubscription);
        #endregion
    }
}
