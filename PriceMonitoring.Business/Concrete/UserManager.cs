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
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IResult Add(User user)
        {
            var isUserExist = IsUserExistInDatabase(user: user);
            var userValidationResult = IsUserValid(user: user);
            var sb = new StringBuilder();
            if (isUserExist)
            {
                return new ErrorResult(message: Messages.UserIsExist);
            }

            if (!userValidationResult.IsValid)
            {
                userValidationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            _unitOfWork.Users.Add(entity: user);
            _unitOfWork.Save();
            return new SuccessResult(message: Messages.UserRegistered);
        }

        public async Task<IResult> AddAsync(User user)
        {
            var isUserExist = IsUserExistInDatabase(user: user);
            var userValidationResult = IsUserValid(user: user);
            var sb = new StringBuilder();
            if (isUserExist)
            {
                return new ErrorResult(message: Messages.UserIsExist);
            }

            if (!userValidationResult.IsValid)
            {
                userValidationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            await _unitOfWork.Users.AddAsync(entity: user);
            await _unitOfWork.SaveAsync();
            return new SuccessResult(message: Messages.UserRegistered);
        }

        public IResult Delete(int id)
        {
            var isUserExist = _unitOfWork.Users.Get(x => x.Id == id) == null;
            if (isUserExist)
            {
                _unitOfWork.Users.Delete(new User { Id = id });
                _unitOfWork.Save();
                return new SuccessResult(message: Messages.UserDeleted);
            }

            return new ErrorResult(message: Messages.UserIsNotExist);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var isUserExist = await _unitOfWork.Users.GetAsync(x => x.Id == id) == null;
            if (isUserExist)
            {
                await _unitOfWork.Users.DeleteAsync(new User { Id = id });
                await _unitOfWork.SaveAsync();
                return new SuccessResult(message: Messages.UserDeleted);
            }

            return new ErrorResult(message: Messages.UserIsNotExist);
        }

        public IDataResult<User> GetByEmail(string email)
        {
            var user = _unitOfWork.Users.Get(x => x.Email == email);
            if (user == null)
            {
                return new ErrorDataResult<User>(message: Messages.UserIsNotExist);
            }

            return new SuccessDataResult<User>(data: user, message: Messages.UserListed);
        }

       
        public async Task<IDataResult<User>> GetByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetAsync(x => x.Email == email);
            if (user == null)
            {
                return new ErrorDataResult<User>(message: Messages.UserIsNotExist);
            }

            return new SuccessDataResult<User>(data: user, message: Messages.UserListed);
        }

        public IDataResult<User> GetById(int id)
        {
            var user = _unitOfWork.Users.Get(x => x.Id == id);
            if (user == null)
            {
                return new ErrorDataResult<User>(message: Messages.UserIsNotExist);
            }

            return new SuccessDataResult<User>(data: user, message: Messages.UserListed);
        }

        public async Task<IDataResult<User>> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetAsync(x => x.Id == id);
            if (user == null)
            {
                return new ErrorDataResult<User>(message: Messages.UserIsNotExist);
            }

            return new SuccessDataResult<User>(data: user, message: Messages.UserListed);
        }

        public IResult Update(User user)
        {
            var isUserExist = IsUserExistInDatabase(user: user);
            var userValidationResult = IsUserValid(user: user);
            var sb = new StringBuilder();
            if (!isUserExist)
            {
                return new ErrorResult(message: Messages.UserIsNotExist);
            }

            if (!userValidationResult.IsValid)
            {
                userValidationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            _unitOfWork.Users.Update(entity: user);
            _unitOfWork.Save();
            return new SuccessResult(message: Messages.UserUpdated);
        }

        public async Task<IResult> UpdateAsync(User user)
        {
            var isUserExist = IsUserExistInDatabase(user: user);
            var userValidationResult = IsUserValid(user: user);
            var sb = new StringBuilder();
            if (!isUserExist)
            {
                return new ErrorResult(message: Messages.UserIsNotExist);
            }

            if (!userValidationResult.IsValid)
            {
                userValidationResult.Errors.ForEach(x => sb.Append(x.ErrorMessage));
                return new ErrorResult(message: sb.ToString());
            }

            await _unitOfWork.Users.UpdateAsync(entity: user);
            await _unitOfWork.SaveAsync();
            return new SuccessResult(message: Messages.UserUpdated);
        }

        public bool IsUserExistInDatabase(User user)
        {
            return _unitOfWork.Users.Get(x => x.Email == user.Email) != null;
        }

        public ValidationResult IsUserValid(User user)
        {
            return ValidationTool.Validate(new UserValidator(), user);
        }
    }
}
