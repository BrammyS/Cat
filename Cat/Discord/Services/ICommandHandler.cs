using System.Threading.Tasks;
using Discord.WebSocket;

namespace Cat.Discord.Services
{
    public interface ICommandHandler
    {
        Task InitializeAsync(DiscordShardedClient client);

    }
}
