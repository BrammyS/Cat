using Cat.Persistence.Domain.Tables;
using Cat.Persistence.EntityFrameworkCore.Models;
using Cat.Persistence.Interfaces.Repositories;

namespace Cat.Persistence.EntityFrameworkCore.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CatContext context) : base(context)
        {
        }
    }
}