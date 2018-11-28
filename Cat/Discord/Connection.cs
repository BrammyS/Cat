using System.Threading.Tasks;
using Cat.Configurations;
using Discord;
using Discord.WebSocket;
using IConnection = Cat.Discord.Interfaces.IConnection;

namespace Cat.Discord
{
    public class Connection : IConnection
    {
        private readonly DiscordShardedClient _client;

        public Connection(DiscordShardedClient client)
        {
            _client = client;
        }
        
        public async Task ConnectAsync()
        {
            await _client.LoginAsync(TokenType.Bot, ConfigData.Data.Token).ConfigureAwait(false);
            await _client.StartAsync().ConfigureAwait(false);

            await Task.Delay(ConfigData.Data.RestartTime * 60000).ConfigureAwait(false);
            await _client.StopAsync().ConfigureAwait(false);
        }
    }
}
