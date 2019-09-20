using Discord.WebSocket;

namespace Cat.Discord.Interfaces
{
    public interface IUserLeftHandler
    {
        void Initialize(DiscordShardedClient client);

    }
}