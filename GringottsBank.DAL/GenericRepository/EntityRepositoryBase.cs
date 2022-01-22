using GringottsBank.Core.Abstract;
using GringottsBank.Core.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.DAL.GenericRepository
{
    public class EntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
             where TEntity : class, IEntity, new()
             where TContext : DbContext, new()
    {
        protected TContext _dbContext;

        public EntityRepositoryBase(TContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            EntityPaging paging = null)
        {
            var query = _dbContext.Set<TEntity>().Where(filter);
            if (includes != null)
            {
                query = includes(query);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            EntityPaging paging = null)
        {
            var query = Query(filter, orderBy, includes, paging);

            return await query.ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        public void BeginAdd(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        public void BeginUpdate(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }


        public async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }
        public void BeginDelete(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }


        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await _dbContext.Set<TEntity>().CountAsync(filter == null ? x => true : filter);
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(filter);
        }

        #region Utilities

        protected IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            EntityPaging paging = null)
        {

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (paging != null)
            {
                int itemCount = paging.ItemCount.HasValue ? paging.ItemCount.Value : 0;
                if (paging.PageNumber.HasValue)
                {
                    int pageNumber = paging.PageNumber.Value <= 1 ? 1 : paging.PageNumber.Value;
                    query = query.Skip((pageNumber - 1) * itemCount);
                }
                if (itemCount > 0)
                {
                    query = query.Take(itemCount);
                }
            }

            return query;

        }
        #endregion
    }
}
