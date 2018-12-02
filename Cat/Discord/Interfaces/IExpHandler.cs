using Discord.WebSocket;

namespace Cat.Discord.Interfaces
{
    public interface IExpHandler
    {
        void Initialize(DiscordShardedClient client);
    }
}