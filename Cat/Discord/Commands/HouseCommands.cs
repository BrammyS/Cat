using System;
using System.Linq;
using System.Threading.Tasks;
using Cat.Services;
using Discord;
using Discord.Commands;

namespace Cat.Discord.Commands
{
    public class HouseCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger _logger;
        private readonly EmbedBuilder _embed;

        public HouseCommands(ILogger logger)
        {
            _logger = logger;
            _embed = new EmbedBuilder();
        }


        [Command("house members", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task ShowHouseMembers()
        {
            try
            {
                var houseIds = new []
                               {
                                   Constants.RoleIds.OrderOfDoctrina,
                                   Constants.RoleIds.OrderOfHostilia,
                                   Constants.RoleIds.OrderOfSocius,
                                   Constants.RoleIds.OrderOfReciprocus
                               };
                foreach (var houseId in houseIds)
                {
                    var role = Context.Client.GetGuild(Constants.GuildIds.Los).GetRole(houseId);
                    var listedUsers = role.Members.OrderBy(x=>x.Nickname).Aggregate("", (current, member) => current + $"{Context.Client.GetUser(member.Id).Mention}, ");
                    _embed.WithTitle(role.Name);
                    _embed.WithDescription(listedUsers.Remove(listedUsers.Length - 2));
                    _embed.WithFooter($"Member count: {role.Members.Count()}");
                    await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
                }

                _logger.Log($"Server: {Context.Guild}, Id: {Context.Guild.Id} || ShardId: {Context.Client.ShardId} || Channel: {Context.Channel} || User: {Context.User} || Used: house members");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}