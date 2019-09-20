using Discord;
using Discord.WebSocket;

namespace Cat.Discord.Configurations
{
    public static class SocketConfig
    {
        public static DiscordSocketConfig GetDefault()
        {
            return new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 999,
                ExclusiveBulkDelete = true
            };
        }
    }
}
