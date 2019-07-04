using System;
using System.Linq;
using System.Threading.Tasks;
using Cat.Discord.Interfaces;
using Cat.Discord.Services;
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
            _client.ReactionAdded += ReactionAddedEvent;
            _client.MessageReceived += MessageReceivedEvent;
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
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var user = await unitOfWork.Users.GetOrAddUserInfoAsync(guildUser.Guild.Id, guildUser.Id, guildUser.Username).ConfigureAwait(false);
                if (oldState.VoiceChannel == null) user.LastVoiceStateUpdateReceived = DateTime.Now;
                else if (newState.VoiceChannel == null && oldState.VoiceChannel.Id != Constants.ChannelIds.VoiceChannelIds.Afk)
                {
                    if(oldState.IsSelfMuted || oldState.IsSelfDeafened) return;
                    var timeDiff = (decimal)DateTime.Now.Subtract(user.LastVoiceStateUpdateReceived).TotalMinutes;
                    user.TimeConnected += timeDiff;
                    await _expService.GiveXp(timeDiff / 5, user, guildUser.Guild, socketUser, unitOfWork).ConfigureAwait(false);
                }

                await unitOfWork.SaveAsync().ConfigureAwait(false);
            }
        }

        private async Task HandleBumUpAsync(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage message)) return;
            if (message.Author.IsBot) return;
            if(message.Channel.Id == 377895556204331008) return;
            var context = new ShardedCommandContext(_client, message);
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var guildUser = context.Guild.GetUser(context.User.Id);
                if (!guildUser.JoinedAt.HasValue) return;
                var userRoleIds = guildUser.Roles.Select(x => x.Id).ToList();
                var user = await unitOfWork.Users.GetUserAsync(context.Guild.Id, context.User.Id).ConfigureAwait(false);
                if(user == null) return;
                
                
                if(!userRoleIds.Contains(Constants.RoleIds.NewbieRoster) && !userRoleIds.Contains(Constants.RoleIds.PaperRoster)) return;
                // newbie to reg
                if (DateTime.Now.Subtract(guildUser.JoinedAt.Value.Date).TotalDays > 30 && (user.MessagesSend > 1000 || user.TimeConnected > 420) && userRoleIds.Contains(Constants.RoleIds.NewbieRoster))
                {
                    await guildUser.RemoveRoleAsync(context.Guild.GetRole(Constants.RoleIds.NewbieRoster)).ConfigureAwait(false);
                    await guildUser.AddRoleAsync(context.Guild.GetRole(Constants.RoleIds.RegularRoster)).ConfigureAwait(false);
                    await context.Guild.GetTextChannel(Constants.ChannelIds.TextChannelIds.RegularPub).SendMessageAsync($"{context.User.Mention} You are now on the regular roster. :tada:").ConfigureAwait(false);
                }

                // paper to newbie
                else if (DateTime.Now.Subtract(guildUser.JoinedAt.Value.Date).TotalDays > 7 && (user.MessagesSend > 100 || user.TimeConnected > 60) && userRoleIds.Contains(Constants.RoleIds.PaperRoster))
                {
                    await guildUser.RemoveRoleAsync(context.Guild.GetRole(Constants.RoleIds.PaperRoster)).ConfigureAwait(false);
                    await guildUser.AddRoleAsync(context.Guild.GetRole(Constants.RoleIds.NewbieRoster)).ConfigureAwait(false);
                    await context.Guild.GetTextChannel(Constants.ChannelIds.TextChannelIds.Bot).SendMessageAsync($"{context.User.Mention} You are now on the newbie roster. :tada:").ConfigureAwait(false);

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
