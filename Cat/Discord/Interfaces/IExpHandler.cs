using System.Threading.Tasks;
using Discord.WebSocket;

namespace Cat.Discord.Interfaces
{
    public interface IExpHandler
    {
        Task InitializeAsync(DiscordShardedClient client);
    }
}