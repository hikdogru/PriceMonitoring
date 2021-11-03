using Microsoft.EntityFrameworkCore;
using PriceMonitoring.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Core.Data.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new() where TContext: DbContext, new()
    {
        public Task Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
