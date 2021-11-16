using FluentValidation.Results;
using PriceMonitoring.Core.Utilities.Results;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.Abstract
{
    public interface IUserService
    {
        #region sync

        IDataResult<User> GetByEmail(string email);
        IDataResult<User> GetById(int id);
        IResult Add(User user);
        IResult Update(User user);
        IResult Delete(int id);

        #endregion


        #region async

        Task<IDataResult<User>> GetByEmailAsync(string email);
        Task<IDataResult<User>> GetByIdAsync(int id);
        Task<IResult> AddAsync(User user);
        Task<IResult> UpdateAsync(User user);
        Task<IResult> DeleteAsync(int id);
        ValidationResult IsUserValid(User user);
        bool IsUserExistInDatabase(User user);

        #endregion
    }
}
