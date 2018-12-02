using System.Threading.Tasks;
using Cat.Persistence.Domain.Tables;

namespace Cat.Persistence.Interfaces.Repositories
{
    public interface IUserInfoRepository : IRepository<UserInfo>
    {
        Task<UserInfo> GetUserInfoAsync(decimal serverId, decimal userId);
        Task<UserInfo> GetOrAddUserInfoAsync(decimal serverId, decimal userId);
    }
}
