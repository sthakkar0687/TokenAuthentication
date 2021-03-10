using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TokenAuthentication.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(T[] entities);
        void AddRange(IEnumerable<T> entities);

        void Remove(object id);        
        void Remove(T entity);
        void RemoveRange(T[] entities);
        void RemoveRange(IEnumerable<T> entities);

        void Update(T entity);

        IEnumerable<T> GetList();
        IEnumerable<T> GetList(Expression<Func<T, bool>> expression);

        T FirstOrDefault(Expression<Func<T, bool>> expression);


        Task AddAsync(T entity);
        Task AddRangeAsync(T[] entities);
        Task AddRangeAsync(IEnumerable<T> entities);

        Task RemoveAsync(T entity);
        Task RemoveAsync(object id);

        Task<IEnumerable<T>> GetListAsync();
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

    }
}
