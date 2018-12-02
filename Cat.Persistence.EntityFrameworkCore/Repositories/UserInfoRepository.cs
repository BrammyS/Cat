using System.Linq;
using System.Threading.Tasks;
using Cat.Persistence.Domain.Tables;
using Cat.Persistence.EntityFrameworkCore.Models;
using Cat.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cat.Persistence.EntityFrameworkCore.Repositories
{
    public class UserInfoRepository : Repository<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(CatContext context) : base(context)
        {

        }

        public async Task<UserInfo> GetUserInfoAsync(decimal serverId, decimal userId)
        {
            return await Context.Set<UserInfo>().FirstOrDefaultAsync(x=>x.ServerId == serverId  && x.UserId == userId);
        }
    }
}