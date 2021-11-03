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
        private readonly TContext _context;
        private DbSet<TEntity> Table { get; set; }
        public EfEntityRepositoryBase()
        {
            _context = new();
            Table = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            Table.Remove(entity);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Table.SingleOrDefaultAsync(filter);
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ?  Table : Table.Where(filter);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            Table.Update(entity);
        }
    }
}
