using System;
using System.Threading.Tasks;
using Cat.Persistence.Interfaces.UnitOfWork;
using Discord;
using Discord.Commands;

namespace Cat.Discord.Commands
{

    public class LogCommands : ModuleBase<SocketCommandContext>
    {

        private readonly IUnitOfWork _unitOfWork;


        public LogCommands(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [Command("r", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task TopUsersAsync(decimal logId, [Remainder] string reason)
        {
            var log = _unitOfWork.Logs.Get(logId);
            if (log == null)
            {
                var errorMessage = await ReplyAsync("Log not found!").ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
                await errorMessage.DeleteAsync().ConfigureAwait(false);
                await Context.Message.DeleteAsync().ConfigureAwait(false);
                return;
            }
            var logMessage = await Context.Guild.GetTextChannel(Constants.ChannelIds.TextChannelIds.LogChannel).GetMessageAsync((ulong) log.MessageId).ConfigureAwait(false);
            if (!(logMessage is IUserMessage msg)) return;
            var newEmbed =  new EmbedBuilder();
            foreach (var embed in logMessage.Embeds)
            {
                newEmbed.WithTitle(embed.Title);
                if (embed.Timestamp.HasValue) newEmbed.WithTimestamp(embed.Timestamp.Value);
                if (embed.Color.HasValue) newEmbed.WithColor(embed.Color.Value);
                if (!embed.Fields.IsDefaultOrEmpty)
                {
                    foreach (var field in embed.Fields)
                    {
                        if (field.Name != "Reason") newEmbed.AddField(new EmbedFieldBuilder
                                                                      {
                                                                          Value = field.Value,
                                                                          Name = field.Name,
                                                                          IsInline = field.Inline,
                                                                      });
                    }
                }
                if (embed.Author.HasValue) newEmbed.WithAuthor(new EmbedAuthorBuilder
                                                               {
                                                                   Name = embed.Author.Value.Name,
                                                                   IconUrl = embed.Author.Value.IconUrl
                                                               });
            }

            newEmbed.AddField(new EmbedFieldBuilder
                              {
                                  Name = "Reason",
                                  Value = reason,
                                  IsInline = true
                              });
            await msg.ModifyAsync(x =>
            {
                x.Embed = new Optional<Embed>(newEmbed.Build());
            }).ConfigureAwait(false);
            await Context.Message.DeleteAsync().ConfigureAwait(false);
        }
    }
}
