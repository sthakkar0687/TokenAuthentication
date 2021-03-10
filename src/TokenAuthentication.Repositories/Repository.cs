using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TokenAuthentication.Interfaces;

namespace TokenAuthentication.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(T entity)
        {
            _dbContext.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public void AddRange(T[] entities)
        {
            _dbContext.AddRange(entities);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbContext.AddRange(entities);
        }

        public async Task AddRangeAsync(T[] entities)
        {
            await _dbContext.AddRangeAsync(entities);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.AddRangeAsync(entities);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().FirstOrDefault(expression);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public IEnumerable<T> GetList()
        {
            return _dbContext.Set<T>().ToList();
        }

        public IEnumerable<T> GetList(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression).ToList();
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public void Remove(T entity)
        {
            //var existing = _dbContext.Set<T>().Find(entity);
            if (entity != null)
            {
                PropertyInfo deletedProp = entity.GetType().GetProperty("IsDeleted") ?? entity.GetType().GetProperty("Deleted");
                if (deletedProp != null)
                {
                    deletedProp.SetValue(entity, true);
                    Update(entity);
                }
                else
                {
                    _dbContext.Set<T>().Remove(entity);
                }
            }
        }

        public void Remove(object id)
        {
            var existing = _dbContext.Set<T>().Find(id);
            if (existing != null)
                Remove(existing);
            //var typeInfo = typeof(T).GetTypeInfo();
            //var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            //var property = typeInfo.GetProperty(key?.Name ?? throw new InvalidOperationException());
            //if (property != null)
            //{
            //    var entity = Activator.CreateInstance<T>();
            //    if (entity != null)
            //        Remove(entity);
            //    //property.SetValue(entity, id);
            //    //_dbContext.Entry(entity).State = EntityState.Deleted;
            //}
            //else
            //{
            //    var existing = _dbContext.Set<T>().Find(id);
            //    if (existing != null)
            //        Remove(existing);
            //}
        }

        public async Task RemoveAsync(T entity)
        {
            //var existing = await _dbContext.Set<T>().FindAsync(entity);
            if (entity != null)
            {
                PropertyInfo deletedProp = entity.GetType().GetProperty("IsDeleted") ?? entity.GetType().GetProperty("Deleted");
                if (deletedProp != null)
                {
                    deletedProp.SetValue(entity, true);
                    Update(entity);
                }
                else
                {
                    _dbContext.Set<T>().Remove(entity);
                }
            }
        }

        public async Task RemoveAsync(object id)
        {
            var existing = _dbContext.Set<T>().Find(id);
            if (existing != null)
                await RemoveAsync(existing);
            //var deleteEntity = _dbContext.Set<T>().Find(id);
            //var typeInfo = typeof(T).GetTypeInfo();
            //var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            //var property = typeInfo.GetProperty(key?.Name ?? throw new InvalidOperationException());
            //if (property != null)
            //{
            //    var entity = Activator.CreateInstance<T>();
            //    property.SetValue(entity, id);
            //    _dbContext.Entry<T>(entity).State = EntityState.Deleted;
            //    //await RemoveAsync(entity);
            //    //property.SetValue(entity, id);
            //    //_dbContext.Entry(entity).State = EntityState.Deleted;
            //}
            //else
            //{
            //    var existing = _dbContext.Set<T>().Find(id);
            //    if (existing != null)
            //        await RemoveAsync(existing);
            //}
        }

        public void RemoveRange(T[] entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (predicate != null)
                query = query.Where(predicate);
            if (include != null)
                query = include(query);
            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();

        }
    }
}
