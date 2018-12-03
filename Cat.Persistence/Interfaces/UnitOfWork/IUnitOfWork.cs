using System;
using System.Threading.Tasks;
using Cat.Persistence.Interfaces.Repositories;

namespace Cat.Persistence.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IServerRepository Servers { get; }
        IUserRepository Users { get; }
        int Save();
        Task<int> SaveAsync();
    }
}