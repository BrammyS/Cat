using System.Threading.Tasks;
using Discord.WebSocket;

namespace Cat.Discord.Interfaces
{
    public interface IMessageHandler
    {
        Task InitializeAsync(DiscordShardedClient client);
    }
}