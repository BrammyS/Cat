using System.Threading.Tasks;
using Cat.Discord.Interfaces;
using Cat.Discord.Services;
using Cat.Persistence.Interfaces.UnitOfWork;
using Discord.WebSocket;


namespace Cat.Discord.Handlers
{

    public class UserLeftHandler: IUserLeftHandler
    {

        private readonly IUnitOfWork _unitOfWork;
        private DiscordShardedClient _client;

        public UserLeftHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Initialize(DiscordShardedClient client)
        {
            _client = client;
            _client.UserLeft += UserLeftEvent;
        }

        private Task UserLeftEvent(SocketGuildUser guildUser)
        {
            Task.Run(async () => await UserLeft(guildUser).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private async Task UserLeft(SocketGuildUser guildUser)
        {
            if (guildUser.Guild.Id != Constants.GuildIds.Los) return;
            var user = await _unitOfWork.Users.GetUserAsync(guildUser.Guild.Id, guildUser.Id).ConfigureAwait(false);
            user.Level = 0;
            user.MessagesSend = 0;
            user.TimeConnected = 0;
            user.Xp = 0;
        }
    }
}
