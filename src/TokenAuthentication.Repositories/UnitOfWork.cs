using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenAuthentication.Interfaces;

namespace TokenAuthentication.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        public TContext Context { get; }
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }        

        public void Dispose()
        {
            Context?.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
                _repositories = new Dictionary<Type, object>();
            var entityType = typeof(TEntity);
            if (!_repositories.ContainsKey(entityType))
                _repositories[entityType] = new Repository<TEntity>(Context);
            return (IRepository<TEntity>)_repositories[entityType];
        }

        public int SaveAndCommit()
        {
            return Context.SaveChanges();
        }

        public Task<int> SaveAndCommitAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}
