using System;
using System.Threading.Tasks;
using Cat.Discord.Interfaces;
using Cat.Persistence.Domain.Tables;
using Cat.Persistence.Interfaces.UnitOfWork;
using Cat.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Cat.Discord.Handlers
{
    public class ExpHandler : IExpHandler
    {
        private DiscordShardedClient _client;
        private readonly ILogger _logger;

        public ExpHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void Initialize(DiscordShardedClient client)
        {
            _client = client;
            _client.ReactionAdded += ReactionAdded;
            _client.MessageReceived += MessageReceived;
            _client.UserVoiceStateUpdated += UserVoiceStateUpdated;
        }

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> cacheAbleMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            await ReactionAddedAsync(reaction).ConfigureAwait(false);
        }

        private async Task ReactionAddedAsync(SocketReaction reaction)
        {
            if (!(reaction.Channel is SocketGuildChannel guildChannel)) return;
            if(reaction.User.Value.IsBot) return;
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var user = await unitOfWork.UserInfos.GetOrAddUserInfoAsync(guildChannel.Guild.Id, reaction.UserId).ConfigureAwait(false);
                await GiveXp(1, user, guildChannel.Guild, reaction.User.Value, unitOfWork).ConfigureAwait(false);
                await unitOfWork.SaveAsync().ConfigureAwait(false);
            }
        }

        private async Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            await UserVoiceStateUpdatedAsync(user, state1, state2).ConfigureAwait(false);
        }

        private async Task UserVoiceStateUpdatedAsync(SocketUser socketUser, SocketVoiceState oldState, SocketVoiceState newState)
        {
            if(!(socketUser is SocketGuildUser guildUser)) return;
            if(guildUser.IsBot) return;
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var user = await unitOfWork.UserInfos.GetOrAddUserInfoAsync(guildUser.Guild.Id, guildUser.Id).ConfigureAwait(false);
                if (oldState.VoiceChannel == null) user.LastVoiceStateUpdateReceived = DateTime.Now;
                if (newState.VoiceChannel == null)
                {
                    var timeDiff = (decimal) DateTime.Now.Subtract(user.LastVoiceStateUpdateReceived).TotalMilliseconds;
                    user.TimeConnected += timeDiff;
                    await GiveXp(timeDiff / 5, user, guildUser.Guild, socketUser, unitOfWork).ConfigureAwait(false);
                }

                await unitOfWork.SaveAsync().ConfigureAwait(false);
            }
        }

        private async Task MessageReceived(SocketMessage msg)
        {
            await MessageReceivedAsync(msg).ConfigureAwait(false);
        }

        private async Task MessageReceivedAsync(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage message)) return;
            if(message.Author.IsBot) return;
            var context = new ShardedCommandContext(_client, message);
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var user = await unitOfWork.UserInfos.GetOrAddUserInfoAsync(context.Guild.Id, context.User.Id).ConfigureAwait(false);
                if (!(DateTime.Now.Subtract(user.LastMessageSend).TotalSeconds > 4)) return;
                await GiveXp(2, user, context.Guild, context.User, unitOfWork).ConfigureAwait(false);
                user.LastMessageSend = DateTime.Now;
                await unitOfWork.SaveAsync().ConfigureAwait(false);
            }
        }

        private async Task GiveXp(decimal xp, UserInfo user, SocketGuild guild, IMentionable socketUser , IUnitOfWork unitOfWork)
        {
            user.Xp += xp;
            var xpNeeded = user.Xp > user.Level * (user.Level + 25);
            if (xpNeeded)
            {
                var server = await unitOfWork.Servers.GetOrAddServerAsync(guild.Id, guild.Name, guild.MemberCount).ConfigureAwait(false);
                while (xpNeeded)
                {
                    user.Level++;
                    xpNeeded = user.Xp > user.Level * (user.Level + 25);
                }
                if (server.LevelUpChannel != null)await guild.GetTextChannel((ulong)server.LevelUpChannel).SendMessageAsync($"{socketUser.Mention} just leveled up to lvl: {user.Level} :tada:\n" +
                                                                                                                            "User `?level` for more info.").ConfigureAwait(false);
            }
        }
    }
}
