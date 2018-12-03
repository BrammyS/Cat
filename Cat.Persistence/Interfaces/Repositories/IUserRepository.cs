using System.Collections.Generic;
using System.Threading.Tasks;
using Cat.Persistence.Domain.Tables;

namespace Cat.Persistence.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserAsync(decimal serverId, decimal userId);
        Task<List<User>> GetTopUsers(decimal serverId);
        Task<User> GetOrAddUserInfoAsync(decimal serverId, decimal userId, string userName);
        Task<int> FindPosition(decimal serverId, decimal userId);
    }
}