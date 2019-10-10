using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat.Persistence.Domain.Tables;
using Cat.Persistence.Interfaces.UnitOfWork;
using Discord;
using Discord.WebSocket;

namespace Cat.Discord.Handlers
{

    public class EventMessageHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private DiscordShardedClient _client;

        public EventMessageHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Initialize(DiscordShardedClient client)
        {
            _client = client;
            _client.GuildMemberUpdated += PuddingAdded;
            _client.GuildMemberUpdated += LogMemberUpdate;
            _client.UserBanned += UserBanned;
            _client.UserUnbanned += UserUnbanned;
            _client.MessageDeleted += MessageDeleted;
            _client.UserLeft += UserKicked;
            _client.MessageUpdated += MessageEdited;
            _client.UserUpdated += UserChangedAsync;
        }


        private Task UserKicked(SocketGuildUser guildUser)
        {
            Task.Run(async () => await UserKickedAsync(guildUser).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        // ReSharper disable once UnusedMember.Local
        private Task MessageEdited(Cacheable<IMessage, ulong> oldMessage, SocketMessage newMessage, ISocketMessageChannel channel)
        {
            Task.Run(async () => await MessageEditedAsync(oldMessage, newMessage).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private Task MessageDeleted(Cacheable<IMessage, ulong> message, ISocketMessageChannel channel)
        {
            Task.Run(async () => await MessageDeletedAsync(message).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private Task UserUnbanned(SocketUser user, SocketGuild guild)
        {
            Task.Run(async () => await UserUnBannedAsync(user, guild).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private Task UserBanned(SocketUser user, SocketGuild guild)
        {
            Task.Run(async () => await UserBannedAsync(user, guild).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private Task LogMemberUpdate(SocketGuildUser oldUser, SocketGuildUser newUser)
        {
            Task.Run(async () => await LogMemberUpdateAsync(oldUser, newUser).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private Task PuddingAdded(SocketGuildUser oldUser, SocketGuildUser newUser)
        {
            Task.Run(async () => await PuddingAddedAsync(oldUser, newUser).ConfigureAwait(false));
            return Task.CompletedTask;
        }


        private async Task UserKickedAsync(SocketGuildUser guildUser)
        {
            if (!(guildUser.Guild is IGuild guild)) return;
            if (guild.Id != Constants.GuildIds.Los) return;
            var auditLogs = await guild.GetAuditLogsAsync().ConfigureAwait(false);
            var auditLog = auditLogs.FirstOrDefault(x => x.Action == ActionType.Kick);
            if (auditLog != null)
            {
                Console.WriteLine(DateTimeOffset.Now.CompareTo(auditLog.CreatedAt));
                if (DateTimeOffset.Now.CompareTo(auditLog.CreatedAt) <= 1)
                {
                    Console.WriteLine(auditLog.Action);
                    Console.WriteLine(auditLog.Data);
                    Console.WriteLine(auditLog.Reason);
                    Console.WriteLine(auditLog.User.Username);
                    Console.WriteLine(auditLog.CreatedAt);
                }
            }
        }


        private async Task MessageEditedAsync(Cacheable<IMessage, ulong> oldMessage, SocketMessage newMessage)
        {
            try
            {
                if (!oldMessage.HasValue) return;
                if(newMessage.Content == oldMessage.Value.Content) return;
                var oldMessageContent = oldMessage.Value.Content;
                var newMessageContent = newMessage.Content;
                var embed = new EmbedBuilder
                            {
                                Timestamp = DateTimeOffset.UtcNow,
                                Color = Color.Orange,
                                Author = new EmbedAuthorBuilder
                                         {
                                             Name = $"{GetFullUserName(oldMessage.Value.Author.Username, oldMessage.Value.Author.Discriminator)}",
                                             IconUrl = oldMessage.Value.Author.GetAvatarUrl()
                                         },
                                Fields = new List<EmbedFieldBuilder>
                                         {
                                             new EmbedFieldBuilder
                                             {
                                                 Name = "Old message",
                                                 Value = $"{oldMessageContent}",
                                                 IsInline = true
                                             },
                                             new EmbedFieldBuilder
                                             {
                                                 Name = "Edited message",
                                                 Value = $"{newMessageContent}",
                                                 IsInline = true
                                             },
                                             new EmbedFieldBuilder
                                             {
                                                 Name = "Channel",
                                                 Value = $"{_client.GetGuild(Constants.GuildIds.Los).GetTextChannel(newMessage.Channel.Id).Mention}",
                                                 IsInline = false
                                             }
                                         }
                            };
                await _client.GetGuild(Constants.GuildIds.Los)
                             .GetTextChannel(Constants.ChannelIds.TextChannelIds.LogChannel)
                             .SendMessageAsync("", false, embed.Build()).ConfigureAwait(false);
            }
            catch (ArgumentException)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task MessageDeletedAsync(Cacheable<IMessage, ulong> message)
        {
            try
            {
                if(!message.HasValue) return;
                if(message.Value.Author.IsWebhook || (message.Value.Author.IsBot && message.Value.Author.Id != Constants.BotIds.Cat) || message.Value.Content.Contains("?r", StringComparison.CurrentCultureIgnoreCase)) return;
                
                var firstAttachment = message.Value.Attachments.FirstOrDefault();
                if (firstAttachment != null && (firstAttachment.ProxyUrl.Contains(".png") 
                                                || firstAttachment.ProxyUrl.Contains(".jpg") 
                                                || firstAttachment.ProxyUrl.Contains(".webp")
                                                || firstAttachment.ProxyUrl.Contains(".webm")
                                                || firstAttachment.ProxyUrl.Contains(".mp4")
                                                || firstAttachment.ProxyUrl.Contains(".gif") 
                                                || firstAttachment.ProxyUrl.Contains(".jpeg")))
                {
                    var newEmbed = new EmbedBuilder
                                   {
                                       Timestamp = DateTimeOffset.UtcNow,
                                       Color = Color.Orange,
                                       Title = "Image deleted",
                                       Description = message.Value.Content,
                                       Url = firstAttachment.ProxyUrl,
                                       ImageUrl = firstAttachment.ProxyUrl,
                                       Author = new EmbedAuthorBuilder
                                                {
                                                    Name = $"{GetFullUserName(message.Value.Author.Username, message.Value.Author.Discriminator)}",
                                                    IconUrl = message.Value.Author.GetAvatarUrl()
                                                },
                                       Fields = new List<EmbedFieldBuilder>
                                                {
                                                    new EmbedFieldBuilder
                                                    {
                                                        Name = "Channel",
                                                        Value = $"{_client.GetGuild(Constants.GuildIds.Los).GetTextChannel(message.Value.Channel.Id).Mention}",
                                                        IsInline = true
                                                    }
                                                }
                                   };
                    await _client.GetGuild(Constants.GuildIds.Los)
                                 .GetTextChannel(Constants.ChannelIds.TextChannelIds.LogChannel)
                                 .SendMessageAsync("", false, newEmbed.Build()).ConfigureAwait(false);
                }
                else
                {
                    var newEmbed = new EmbedBuilder
                                   {
                                       Timestamp = DateTimeOffset.UtcNow,
                                       Color = Color.Orange,
                                       Title = "Message deleted",
                                       Description = message.Value.Content,
                                       Author = new EmbedAuthorBuilder
                                                {
                                                    Name = $"{GetFullUserName(message.Value.Author.Username, message.Value.Author.Discriminator)}",
                                                    IconUrl = message.Value.Author.GetAvatarUrl()
                                                },
                                       Fields = new List<EmbedFieldBuilder>
                                                {
                                                    new EmbedFieldBuilder
                                                    {
                                                        Name = "Channel",
                                                        Value = $"{_client.GetGuild(Constants.GuildIds.Los).GetTextChannel(message.Value.Channel.Id).Mention}",
                                                        IsInline = true
                                                    }
                                                }
                                   };
                    await _client.GetGuild(Constants.GuildIds.Los)
                                 .GetTextChannel(Constants.ChannelIds.TextChannelIds.LogChannel)
                                 .SendMessageAsync("", false, newEmbed.Build()).ConfigureAwait(false);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task UserUnBannedAsync(SocketUser user, SocketGuild socketGuild)
        {

            try
            {
                if (!(socketGuild is IGuild guild)) return;
                if (guild.Id != Constants.GuildIds.Los) return;
                var auditLogs = await guild.GetAuditLogsAsync().ConfigureAwait(false);
                var auditLog = auditLogs.FirstOrDefault(x => x.Action == ActionType.Unban);
                if (auditLog == null) return;
                var message = await _client.GetGuild(Constants.GuildIds.Los)
                                           .GetTextChannel(Constants.ChannelIds.TextChannelIds.LogChannel)
                                           .SendMessageAsync("", false, GetEmbeddedLog(GetFullUserName(user.Username, user.Discriminator),
                                                                                       user.GetAvatarUrl(),
                                                                                       $"User {user.Mention} got unbanned",
                                                                                       auditLog.User.Mention,
                                                                                       auditLog.Id,
                                                                                       Color.Green,
                                                                                       auditLog.Reason).Build()).ConfigureAwait(false);
                await _unitOfWork.Logs.AddAsync(new Log
                                                {
                                                    LogId = auditLog.Id,
                                                    MessageId = message.Id
                                                }).ConfigureAwait(false);
                await _unitOfWork.SaveAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task UserBannedAsync(SocketUser user, SocketGuild socketGuild)
        {
            
            try
            {
                if (!(socketGuild is IGuild guild)) return;
                if (guild.Id != Constants.GuildIds.Los) return;
                var auditLogs = await guild.GetAuditLogsAsync().ConfigureAwait(false);
                var auditLog = auditLogs.FirstOrDefault(x => x.Action == ActionType.Ban);
                if (auditLog == null) return;
                var message = await _client.GetGuild(Constants.GuildIds.Los)
                                           .GetTextChannel(Constants.ChannelIds.TextChannelIds.LogChannel)
                                           .SendMessageAsync("", false, GetEmbeddedLog(GetFullUserName(user.Username, user.Discriminator),
                                                                                       user.GetAvatarUrl(),
                                                                                       $"User {user.Mention} got banned",
                                                                                       auditLog.User.Mention,
                                                                                       auditLog.Id,
                                                                                       Color.Red,
                                                                                       auditLog.Reason).Build()).ConfigureAwait(false);
                await _unitOfWork.Logs.AddAsync(new Log
                                                {
                                                    LogId = auditLog.Id,
                                                    MessageId = message.Id
                                                }).ConfigureAwait(false);
                await _unitOfWork.SaveAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task LogMemberUpdateAsync(SocketGuildUser oldUser, SocketGuildUser newUser)
        {
            try
            {
                if (!(oldUser.Guild is IGuild guild)) return;
                if (guild.Id != Constants.GuildIds.Los) return;
                foreach (var role in newUser.Roles)
                {
                    if (oldUser.Roles.Select(x => x.Id).Contains(role.Id)) continue;
                    var auditLogs = await guild.GetAuditLogsAsync().ConfigureAwait(false);
                    var auditLog = auditLogs.FirstOrDefault(x => x.Action == ActionType.MemberRoleUpdated);
                    if (auditLog == null || auditLog.User.IsBot || auditLog.User.IsWebhook) continue;
                    var message = await _client.GetGuild(Constants.GuildIds.Los)
                                           .GetTextChannel(Constants.ChannelIds.TextChannelIds.LogChannel)
                                           .SendMessageAsync("", false, GetEmbeddedLog(GetFullUserName(newUser.Username, newUser.Discriminator),
                                                                                       newUser.GetAvatarUrl(),
                                                                                       $"Received the {role.Mention} role.",
                                                                                       auditLog.User.Mention,
                                                                                       auditLog.Id,
                                                                                       Color.Blue).Build()).ConfigureAwait(false);
                    await _unitOfWork.Logs.AddAsync(new Log
                                                    {
                                                        LogId = auditLog.Id,
                                                        MessageId = message.Id
                                                    }).ConfigureAwait(false);
                    await _unitOfWork.SaveAsync().ConfigureAwait(false);
                }

                foreach (var role in oldUser.Roles)
                {
                    if (newUser.Roles.Select(x => x.Id).Contains(role.Id)) continue;
                    var auditLogs = await guild.GetAuditLogsAsync().ConfigureAwait(false);
                    var auditLog = auditLogs.FirstOrDefault(x => x.Action == ActionType.MemberRoleUpdated);
                    if (auditLog == null || auditLog.User.IsBot || auditLog.User.IsWebhook) continue;
                    var message = await _client.GetGuild(Constants.GuildIds.Los)
                                           .GetTextChannel(Constants.ChannelIds.TextChannelIds.LogChannel)
                                           .SendMessageAsync("", false, GetEmbeddedLog(GetFullUserName(newUser.Username, newUser.Discriminator),
                                                                                       newUser.GetAvatarUrl(),
                                                                                       $"Lost the {role.Mention} role.",
                                                                                       auditLog.User.Mention,
                                                                                       auditLog.Id,
                                                                                       Color.DarkBlue).Build()).ConfigureAwait(false);
                    await _unitOfWork.Logs.AddAsync(new Log
                                                    {
                                                        LogId = auditLog.Id,
                                                        MessageId = message.Id
                                                    }).ConfigureAwait(false);
                    await _unitOfWork.SaveAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task PuddingAddedAsync(SocketGuildUser oldUser, SocketGuildUser newUser)
        {
            try
            {
                if (newUser.Roles.Select(x => x.Id).ToList().Contains(Constants.RoleIds.PuddingId) && !oldUser.Roles.Select(x => x.Id).ToList().Contains(Constants.RoleIds.PuddingId))
                {
                    Console.WriteLine("New pudding");
                    var message = $"{newUser.Mention} welcome to the puddings!\n" +
                                  $"This is our own little safe space for the women in LoS, if you have any questions or anything, feel free to message {_client.GetUser(163341531514667008).Mention} :purple_heart:";
                    await _client.GetGuild(Constants.GuildIds.Los).GetTextChannel(Constants.ChannelIds.TextChannelIds.Pudding).SendMessageAsync(message).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task UserChangedAsync(SocketUser oldUser, SocketUser newUser)
        {
            try
            {
                if (oldUser.Username != newUser.Username || oldUser.Discriminator != newUser.Discriminator)
                {
                    var newEmbed = new EmbedBuilder
                    {
                        Timestamp = DateTimeOffset.UtcNow,
                        Color = Color.Orange,
                        Title = "Username changed",
                        Description = $"User {oldUser.Username} changed to {newUser.Username}",
                        Author = new EmbedAuthorBuilder
                        {
                            Name = $"{GetFullUserName(newUser.Username, newUser.Discriminator)}",
                            IconUrl = newUser.GetAvatarUrl()
                        },
                        Fields = new List<EmbedFieldBuilder>
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Old name",
                                Value = $"{GetFullUserName(oldUser.Username, oldUser.Discriminator)}",
                                IsInline = true
                            },
                             new EmbedFieldBuilder
                            {
                                Name = "New name",
                                Value =  $"{GetFullUserName(newUser.Username, newUser.Discriminator)}",
                                IsInline = true
                            }
                        }
                    };
                    await _client.GetGuild(Constants.GuildIds.Los)
                        .GetTextChannel(Constants.ChannelIds.TextChannelIds.LogChannel)
                        .SendMessageAsync("", false, newEmbed.Build()).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private EmbedBuilder GetEmbeddedLog(string user, string userUrl, string action, string moderator, ulong logId, Color color, string reason = null)
        {
            if (string.IsNullOrEmpty(reason)) reason = $"Please do `?r {logId} reason` to add a reason.";
            return new EmbedBuilder
                   {
                       Title = $"Log id: {logId}",
                       Timestamp = DateTimeOffset.UtcNow,
                       Color = color,
                       Author = new EmbedAuthorBuilder
                                {
                                    Name = user,
                                    IconUrl = userUrl
                                },
                       Fields = new List<EmbedFieldBuilder>
                                {
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Action",
                                        Value = action,
                                        IsInline = false
                                    },
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Moderator",
                                        Value = moderator,
                                        IsInline = true
                                    },
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Reason",
                                        Value = reason,
                                        IsInline = true
                                    }
                                }
                   };
        }

        private string GetFullUserName(string nickname, string disc)
        {
            if (disc.Length >= 4) return nickname + "#" + disc;
            disc = "";
            for (var i = 0; i < 4 - disc.Length; i++) disc += "0";

            return nickname + "#" + disc;
        }
    }
}
