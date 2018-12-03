using System;
using System.Collections.Generic;
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
            return await Context.Set<UserInfo>().FirstOrDefaultAsync(x=>x.ServerId == serverId  && x.UserId == userId).ConfigureAwait(false);
        }

        public async Task<UserInfo> GetOrAddUserInfoAsync(decimal serverId, decimal userId)
        {
            var exists = await Context.Set<UserInfo>().AnyAsync(x => x.ServerId == serverId && x.UserId == userId).ConfigureAwait(false);
            if (exists) return await GetUserInfoAsync(serverId, userId).ConfigureAwait(false);
            var userInfo = await Context.Set<UserInfo>().AddAsync(new UserInfo
            {
                UserId = userId,
                LastMessageSend = DateTime.Now,
                Level = 1,
                MessagesSend = 0,
                ServerId = serverId,
                TimeConnected = 0,
                Xp = 0
            }).ConfigureAwait(false);
            await Context.SaveChangesAsync().ConfigureAwait(false);
            return userInfo.Entity;
        }

        public async Task<List<UserInfo>> GetTopUsers(decimal serverId)
        {
            return await Context.Set<UserInfo>().Where(x => x.ServerId == serverId).OrderByDescending(x => x.Level).ThenByDescending(x => x.Xp).Take(10).ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> FindPosition(decimal serverId, decimal userId)
        {
            var users = await Context.Set<UserInfo>().Where(x=>x.ServerId == serverId).OrderBy(x=>x.Level).ThenBy(x=>x.Xp).ToListAsync().ConfigureAwait(false);
            return users.FindIndex(x => x.ServerId == serverId && x.UserId == userId);
        }
    }
}
