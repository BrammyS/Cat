using System;
using System.Threading.Tasks;
using Cat.Persistence.Interfaces.Repositories;

namespace Cat.Persistence.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IServerRepository Servers { get; }
        IUserInfoRepository UserInfos { get; }
        IUserRepository Users { get; }
        int Save();
        Task<int> SaveAsync();
    }
}