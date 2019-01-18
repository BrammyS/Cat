using System;
using System.Linq;
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
        private const ulong PaperId = 490448298222551042;
        private const ulong NewbieId = 403512698651803648;
        private const ulong RegularId = 405523248634396673;
        public ExpHandler(IExpService expService)
        {
            _expService = expService;
        }

        public void Initialize(DiscordShardedClient client)
        {
            _client = client;
            _client.ReactionAdded += ReactionAddedEvent; ;
            _client.MessageReceived += MessageReceivedEvent; ;
            _client.UserVoiceStateUpdated += UserVoiceStateUpdatedEvent; 
        }

        private Task ReactionAddedEvent(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            Task.Run(async () => await  ReactionAddedAsync(arg3).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private Task MessageReceivedEvent(SocketMessage arg)
        {
            Task.Run(async () => await MessageReceivedAsync(arg).ConfigureAwait(false));
            Task.Run(async () => await HandleBumUpAsync(arg).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private Task UserVoiceStateUpdatedEvent(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            Task.Run(async () => await UserVoiceStateUpdatedAsync(user, state1, state2).ConfigureAwait(false));
            return Task.CompletedTask;
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

        private async Task HandleBumUpAsync(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage message)) return;
            if (message.Author.IsBot) return;
            var context = new ShardedCommandContext(_client, message);
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var guildUser = context.Guild.GetUser(context.User.Id);
                if (!guildUser.JoinedAt.HasValue) return;
                var userRoleIds = guildUser.Roles.Select(x => x.Id).ToList();
                var user = await unitOfWork.Users.GetUserAsync(context.Guild.Id, context.User.Id).ConfigureAwait(false);
                if(user == null) return;
                
                
                if(!userRoleIds.Contains(NewbieId) && !userRoleIds.Contains(PaperId)) return;
                //newbie
                if (DateTime.Now.Subtract(guildUser.JoinedAt.Value.Date).TotalDays > 120 && user.MessagesSend > 3000 && user.TimeConnected > 1440 && userRoleIds.Contains(NewbieId))
                {
                    await guildUser.RemoveRoleAsync(context.Guild.GetRole(NewbieId)).ConfigureAwait(false);
                    await guildUser.AddRoleAsync(context.Guild.GetRole(RegularId)).ConfigureAwait(false);
                }

                //paper
                else if (DateTime.Now.Subtract(guildUser.JoinedAt.Value.Date).TotalDays > 30 && user.MessagesSend > 300 && userRoleIds.Contains(PaperId))
                {
                    await guildUser.RemoveRoleAsync(context.Guild.GetRole(PaperId)).ConfigureAwait(false);
                    await guildUser.AddRoleAsync(context.Guild.GetRole(NewbieId)).ConfigureAwait(false);
                }
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
