using System;
using System.Threading.Tasks;
using Cat.Persistence.Interfaces.UnitOfWork;
using Cat.Services;
using Discord;
using Discord.Commands;

namespace Cat.Discord.Commands
{
    public class LevelCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger _logger;
        private readonly EmbedBuilder _embed;

        public LevelCommands(ILogger logger)
        {
            _logger = logger;
            _embed = new EmbedBuilder();
        }

        [Command("level", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task AddColorAsync()
        {
            try
            {
                using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
                {
                    var user = await unitOfWork.UserInfos.GetOrAddUserInfoAsync(Context.Guild.Id, Context.User.Id).ConfigureAwait(false);
                    var position = await unitOfWork.UserInfos.FindPosition(Context.Guild.Id, Context.User.Id).ConfigureAwait(false);
                    _embed.WithTitle($"Info for {Context.User.Username}");
                    if (user != null)
                    {
                        _embed.AddField("Level", $"{user.Level}", true);
                        _embed.AddField("Xp", $"{user.Xp}", true);
                        _embed.AddField("Next level", $"next level in {user.Level * (user.Level + 25)}Xp", true);
                        _embed.AddField("Total time connected", $"{user.TimeConnected}", true);
                    }

                    _embed.AddField("Join date", $"{Context.Guild.GetUser(Context.User.Id).JoinedAt:MM/dd/yyyy}", true);
                    _embed.AddField("Position", $"{position + 1}", true);
                    await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
                    _logger.Log($"Server: {Context.Guild}, Id: {Context.Guild.Id} || ShardId: {Context.Client.ShardId} || Channel: {Context.Channel} || User: {Context.User} || Used: add");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}