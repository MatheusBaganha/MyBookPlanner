using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Repository.Data;
using MyBookPlanner.Repository.Interfaces;

namespace MyBookPlanner.Repository.Repositorys
{
    public class GenericRepository : IGenericRepository
    {
        private readonly MyBookPlannerDataContext _dbContext;

        public GenericRepository(MyBookPlannerDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ------------------- Retrieval -------------------
        public async Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var globalPredicate = GlobalFilter(predicate);
            return await _dbContext.Set<T>().SingleAsync(globalPredicate);
        }

        public async Task<T> GetSingleOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var globalPredicate = GlobalFilter(predicate);
            return await _dbContext.Set<T>().SingleOrDefaultAsync(globalPredicate);
        }

        public async Task<T> GetFirstAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var globalPredicate = GlobalFilter(predicate);
            return await _dbContext.Set<T>().FirstAsync(globalPredicate);
        }

        public async Task<T> GetFirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var globalPredicate = GlobalFilter(predicate);
            return await _dbContext.Set<T>().FirstOrDefaultAsync(globalPredicate);
        }

        public IQueryable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var globalPredicate = GlobalFilter(predicate);
            return _dbContext.Set<T>().Where(globalPredicate);
        }

        public IQueryable<T> GetList<T>() where T : class
        {
            var globalPredicate = GlobalFilter<T>();
            if (globalPredicate != null)
                return _dbContext.Set<T>().Where(globalPredicate);

            return _dbContext.Set<T>();
        }

        public IQueryable<T> GetPagedList<T>(
        int page = 0,
        int pageSize = 10,
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
    ) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query.Skip(page * pageSize).Take(pageSize);

            return query;
        }



        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _dbContext.Set<T>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _dbContext.Set<T>().CountAsync(predicate);
        }

        // ------------------- Modifications -------------------
        public async Task InsertAsync<T>(T entity, bool saveChanges = true) where T : class
        {
            await _dbContext.Set<T>().AddAsync(entity);
            if (saveChanges)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync<T>(T entity, bool saveChanges = true) where T : class
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            if (saveChanges)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync<T>(T entity, bool saveChanges = true) where T : class
        {
            _dbContext.Set<T>().Remove(entity);
            if (saveChanges)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // ------------------- Global Filter -------------------
        private Expression<Func<T, bool>> GlobalFilter<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            // No global filter applied for now
            return predicate;
        }
    }
}
