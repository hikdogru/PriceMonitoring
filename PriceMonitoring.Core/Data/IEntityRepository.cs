using PriceMonitoring.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Core.Data
{
    public interface IEntityRepository<T> where T: class, IEntity, new()
    {
        Task<IQueryable<T>> GetAll(Expression<Func<T , bool>> filter = null);
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }
}
