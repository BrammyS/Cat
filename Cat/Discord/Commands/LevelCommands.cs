using System.Threading.Tasks;
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
            _embed.WithTitle($"Info for {Context.User.Username}");
            _embed.AddField("Level", "420", true);
            _embed.AddField("Xp", "1530", true);
            _embed.AddField("Next level", "next level in 230Xp", true);
            _embed.AddField("Total time connected", "53214", true);
            _embed.AddField("Join date", $"{Context.Guild.GetUser(Context.User.Id).JoinedAt:MM/dd/yyyy}", true);
            _embed.AddField("Position", "6", true);
            await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
            _logger.Log($"Server: {Context.Guild}, Id: {Context.Guild.Id} || ShardId: {Context.Client.ShardId} || Channel: {Context.Channel} || User: {Context.User} || Used: add");
        }
    }
}