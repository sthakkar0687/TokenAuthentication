using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace TokenAuthentication.Interfaces
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        TContext Context { get; }

        IRepository<T> GetRepository<T>() where T : class;

        Task<int> SaveAndCommitAsync();
        int SaveAndCommit();
    }
}
