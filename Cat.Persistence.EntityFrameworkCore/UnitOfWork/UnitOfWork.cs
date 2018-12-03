using System.Threading.Tasks;
using Cat.Persistence.EntityFrameworkCore.Models;
using Cat.Persistence.Interfaces.Repositories;
using Cat.Persistence.Interfaces.UnitOfWork;

namespace Cat.Persistence.EntityFrameworkCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatContext _context;

        public UnitOfWork(CatContext context, IServerRepository serverRepository, IUserRepository userRepository)
        {
            _context = context;
            Servers = serverRepository;
            Users = userRepository;
        }

        public IServerRepository Servers { get; }
        public IUserRepository Users { get; }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}