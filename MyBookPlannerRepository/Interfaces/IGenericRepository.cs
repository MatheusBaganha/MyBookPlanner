using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyBookPlanner.Repository.Interfaces
{
    public interface IGenericRepository
    {
        // ------------------- Retrieval -------------------
        Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<T> GetSingleOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<T> GetFirstAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<T> GetFirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        IQueryable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class;
        IQueryable<T> GetPagedList<T>(
            int page = 0,
            int pageSize = 10,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
        ) where T : class;
    

        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        // ------------------- Modifications -------------------
        Task InsertAsync<T>(T entity, bool saveChanges = true) where T : class;
        Task UpdateAsync<T>(T entity, bool saveChanges = true) where T : class;
        Task DeleteAsync<T>(T entity, bool saveChanges = true) where T : class;

        // ------------------- Save -------------------
        Task<bool> SaveChangesAsync();
    }
}
