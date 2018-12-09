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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CatContext context) : base(context)
        {
        }

        public async Task<User> GetUserAsync(decimal serverId, decimal userId)
        {
            return await Context.Set<User>().FirstOrDefaultAsync(x => x.ServerId == serverId && x.UserId == userId).ConfigureAwait(false);
        }

        public async Task<User> GetOrAddUserInfoAsync(decimal serverId, decimal userId, string userName)
        {
            var exists = await Context.Set<User>().AnyAsync(x => x.ServerId == serverId && x.UserId == userId).ConfigureAwait(false);
            if (exists) return await GetUserAsync(serverId, userId).ConfigureAwait(false);
            var user = await Context.Set<User>().AddAsync(new User
            {
                UserId = userId,
                LastMessageSend = DateTime.Now,
                Level = 1,
                MessagesSend = 0,
                ServerId = serverId,
                TimeConnected = 0,
                Xp = 0,
                CommandUsed = DateTime.Now.AddSeconds(-5),
                LastVoiceStateUpdateReceived = DateTime.Now,
                SpamWarning = 0,
                Name = userName,
                TimesTimedOut = 0,
                LastEmoteAdded = DateTime.Now
            }).ConfigureAwait(false);
            await Context.SaveChangesAsync().ConfigureAwait(false);
            return user.Entity;
        }

        public async Task<List<User>> GetTopLevelUsersAsync(decimal serverId)
        {
            return await Context.Set<User>().Where(x => x.ServerId == serverId).OrderByDescending(x => x.Level).ThenByDescending(x => x.Xp).Take(9).ToListAsync().ConfigureAwait(false);
        }
        public async Task<List<User>> GetTopTimeConnectedUsersAsync(decimal serverId)
        {
            return await Context.Set<User>().Where(x => x.ServerId == serverId).OrderByDescending(x => x.TimeConnected).Take(9).ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> FindPosition(decimal serverId, decimal userId)
        {
            var users = await Context.Set<User>().Where(x => x.ServerId == serverId).OrderBy(x => x.Level).ThenBy(x => x.Xp).ToListAsync().ConfigureAwait(false);
            return users.FindIndex(x => x.ServerId == serverId && x.UserId == userId);
        }
    }
}