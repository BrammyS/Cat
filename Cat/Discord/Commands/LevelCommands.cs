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
        public async Task LevelAsync()
        {
            try
            {
                using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
                {
                    var user = await unitOfWork.Users.GetUserAsync(Context.Guild.Id, Context.User.Id).ConfigureAwait(false);
                    if (user == null)
                    {
                        _embed.WithDescription("No account found!\n" +
                                               "Pls ude **?level** again.");
                        await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
                        return;
                    }
                    var position = await unitOfWork.Users.FindPosition(Context.Guild.Id, Context.User.Id).ConfigureAwait(false);
                    _embed.WithTitle($"Info for {Context.User.Username}");
                    _embed.AddField("Level", $"{user.Level}", true);
                    _embed.AddField("Xp", $"{user.Xp}", true);
                    _embed.AddField("Next level", $"next level in {user.Level * 25 - user.Xp}Xp", true);
                    _embed.AddField("Total time connected", $"{user.TimeConnected}", true);
                    _embed.AddField("Activity score", ".......", true);
                    _embed.AddField("Position", $"{position + 1}", true);
                    await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
                    _logger.Log($"Server: {Context.Guild}, Id: {Context.Guild.Id} || ShardId: {Context.Client.ShardId} || Channel: {Context.Channel} || User: {Context.User} || Used: level");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("TopUsers", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task TopUsersAsync()
        {
            try
            {
                using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
                {
                    var topUsers = await unitOfWork.Users.GetTopLevelUsersAsync(Context.Guild.Id).ConfigureAwait(false);
                    _embed.AddField("Top levels", "These are the top 9 people with the highest level");
                    for (var i = 0; i < topUsers.Count; i++)
                    {
                        _embed.AddField($"{i + 1}. {topUsers[i].Name}", $"Lvl: {topUsers[i].Level}", true);
                    }
                    _embed.AddField("Top time connected", "These are the top 9 people with the highest total time connected to a voice channel");
                    topUsers = await unitOfWork.Users.GetTopTimeConnectedUsersAsync(Context.Guild.Id).ConfigureAwait(false);
                    for (var i = 0; i < topUsers.Count; i++)
                    {
                        _embed.AddField($"{i + 1}. {topUsers[i].Name}", $"{topUsers[i].TimeConnected} minutes", true);
                    }
                    await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
                    _logger.Log($"Server: {Context.Guild}, Id: {Context.Guild.Id} || ShardId: {Context.Client.ShardId} || Channel: {Context.Channel} || User: {Context.User} || Used: TopUsers");
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
