using System.Threading.Tasks;
using Cat.Persistence.Domain.Tables;
using Cat.Persistence.Interfaces.UnitOfWork;
using Discord;
using Discord.WebSocket;

namespace Cat.Discord.Services
{
    public interface IExpService
    {
        Task GiveXp(decimal xp, User user, SocketGuild guild, IMentionable socketUser, IUnitOfWork unitOfWork);
    }
}