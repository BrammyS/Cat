using System;
using System.Threading.Tasks;
using Cat.Discord.Interfaces;
using Cat.Discord.Services;
using Cat.Persistence.Domain.Tables;
using Cat.Persistence.Interfaces.UnitOfWork;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Cat.Discord.Handlers
{
    public class ExpHandler : IExpHandler
    {
        private readonly IExpService _expService;
        private DiscordShardedClient _client;

        public ExpHandler(IExpService expService)
        {
            _expService = expService;
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

        private async Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            await UserVoiceStateUpdatedAsync(user, state1, state2).ConfigureAwait(false);
        }

        private async Task MessageReceived(SocketMessage msg)
        {
            await MessageReceivedAsync(msg).ConfigureAwait(false);
        }

        private async Task ReactionAddedAsync(SocketReaction reaction)
        {
            if (!(reaction.Channel is SocketGuildChannel guildChannel)) return;
            if (reaction.User.Value.IsBot) return;
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var user = await unitOfWork.Users.GetOrAddUserInfoAsync(guildChannel.Guild.Id, reaction.UserId, reaction.User.Value.Username).ConfigureAwait(false);
                if (!(DateTime.Now.Subtract(user.LastEmoteAdded).TotalSeconds > 15)) return;
                user.LastEmoteAdded = DateTime.Now;
                await _expService.GiveXp(1, user, guildChannel.Guild, reaction.User.Value, unitOfWork).ConfigureAwait(false);
                await unitOfWork.SaveAsync().ConfigureAwait(false);
            }
        }

        private async Task UserVoiceStateUpdatedAsync(SocketUser socketUser, SocketVoiceState oldState, SocketVoiceState newState)
        {
            if (!(socketUser is SocketGuildUser guildUser)) return;
            if (guildUser.IsBot) return;
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var user = await unitOfWork.Users.GetOrAddUserInfoAsync(guildUser.Guild.Id, guildUser.Id, guildUser.Username).ConfigureAwait(false);
                if (oldState.VoiceChannel == null) user.LastVoiceStateUpdateReceived = DateTime.Now;
                if (newState.VoiceChannel == null)
                {
                    var timeDiff = (decimal)DateTime.Now.Subtract(user.LastVoiceStateUpdateReceived).TotalMinutes;
                    user.TimeConnected += timeDiff;
                    await _expService.GiveXp(timeDiff / 2, user, guildUser.Guild, socketUser, unitOfWork).ConfigureAwait(false);
                }

                await unitOfWork.SaveAsync().ConfigureAwait(false);
            }
        }

        private async Task MessageReceivedAsync(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage message)) return;
            if(message.Author.IsBot) return;
            var argPos = 0;
            if(message.HasStringPrefix("?", ref argPos)) return;
            var context = new ShardedCommandContext(_client, message);
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var user = await unitOfWork.Users.GetOrAddUserInfoAsync(context.Guild.Id, context.User.Id, context.User.Username).ConfigureAwait(false);
                if (!(DateTime.Now.Subtract(user.LastMessageSend).TotalSeconds > 4)) return;
                await _expService.GiveXp(2, user, context.Guild, context.User, unitOfWork).ConfigureAwait(false);
                user.LastMessageSend = DateTime.Now;
                user.MessagesSend++;
                await unitOfWork.SaveAsync().ConfigureAwait(false);
            }
        }
    }
}
