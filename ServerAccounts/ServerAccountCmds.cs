using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Cat.ServerAccounts
{
    public class ServerAccountCmds : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = { /*Brammys*/308707063993860116 };
        private Random rnd;

        [Command("SetPrefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetPrefix(string prefix)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();

            var server = ServerAccounts.GetAccount(Context.Guild);
            server.Prefix = prefix;
            ServerAccounts.SaveAccounts(Context.Guild);

            embed.WithDescription($"Custom prefix added\nCommand example: `{prefix}help`");
            embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
