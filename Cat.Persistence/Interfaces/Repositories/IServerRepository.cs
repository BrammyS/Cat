using System.Threading.Tasks;
using Cat.Persistence.Domain.Tables;

namespace Cat.Persistence.Interfaces.Repositories
{
    public interface IServerRepository : IRepository<Server>
    {
        Task<Server> GetOrAddServerAsync(decimal id, string serverName, int memberCount);
        Task<Server> GetServerAsync(decimal id);
    }
}