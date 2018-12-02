using System;
using System.Threading.Tasks;
using Cat.Persistence.Domain.Tables;
using Cat.Persistence.EntityFrameworkCore.Models;
using Cat.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cat.Persistence.EntityFrameworkCore.Repositories
{
    public class ServerRepository : Repository<Server>, IServerRepository
    {
        public ServerRepository(CatContext context) : base(context)
        {
        }

        public async Task<Server> GetServerAsync(decimal id)
        {
            try
            {
                return await Context.Set<Server>().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Server> GetOrAddServerAsync(decimal id, string serverName, int memberCount)
        {
            try
            {
                var exists = await Context.Set<Server>().AnyAsync(x => x.Id == id).ConfigureAwait(false);
                if (exists) return await GetServerAsync(id).ConfigureAwait(false);
                var server = await Context.Set<Server>().AddAsync(new Server
                {
                    Id = id,
                    Name = serverName,
                    Active = true,
                    Prefix = null,
                    JoinDate = DateTime.Now.Date,
                    TotalMembers = memberCount,
                    LevelUpChannel = null
                }).ConfigureAwait(false);
                await Context.SaveChangesAsync().ConfigureAwait(false);
                return server.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}