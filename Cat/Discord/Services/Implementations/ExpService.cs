using System.Threading.Tasks;
using Cat.Persistence.Domain.Tables;
using Cat.Persistence.Interfaces.UnitOfWork;
using Discord;
using Discord.WebSocket;

namespace Cat.Discord.Services.Implementations
{
    public class ExpService : IExpService
    {
        public async Task GiveXp(decimal xp, User user, SocketGuild guild, IMentionable socketUser, IUnitOfWork unitOfWork)
        {
            user.Xp += xp;
            var xpNeeded = user.Xp >= user.Level * (user.Level + 25);
            if (xpNeeded)
            {
                var server = await unitOfWork.Servers.GetOrAddServerAsync(guild.Id, guild.Name, guild.MemberCount).ConfigureAwait(false);
                while (xpNeeded)
                {
                    user.Xp -= user.Level * (user.Level + 25);
                    user.Level++;
                    xpNeeded = user.Xp >= user.Level * (user.Level + 25);
                }

                if (server.LevelUpChannel != null) await guild.GetTextChannel((ulong) server.LevelUpChannel).SendMessageAsync($"{socketUser.Mention} just leveled up to lvl: {user.Level} :tada:\n" +
                                                                                                                              "Use `?level` for more info.").ConfigureAwait(false);
            }
        }
    }
}
