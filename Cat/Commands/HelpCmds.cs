using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Cat.Commands
{
    public class HelpCmds : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = { /*Brammys*/308707063993860116 };
        private Random _rnd;

        [Command("help")]
        public async Task help()
        {
            _rnd = new Random();
            var embed = new EmbedBuilder();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);
            var account = UserAccounts.UserAccounts.GetAccount(Context.User, Context.Guild);
            string prefix;
            if (server.Prefix == null) prefix = "Cat ";
            else prefix = server.Prefix;
            string admintxt = $"\n{prefix}**AdminHelp**";


            embed.WithTitle("Info for " + Context.User.Username);
            embed.WithDescription(":arrow_double_down:       :arrow_double_down: ");
            embed.AddField("Prefix", $"This server uses '{prefix}' for my commands");
            embed.AddField("Usefull commands", $"{prefix}**Level**\n{prefix}**TopUsers**" + admintxt);

            embed.WithColor(new Color(_rnd.Next(255), _rnd.Next(255), _rnd.Next(255)));
            await Context.Channel.SendMessageAsync("", false, embed);
            Console.WriteLine($"{DateTime.Now:G}" + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} || Used: TopUsers");
        }

        [Command("adminHelp")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task adminHelp()
        {
            _rnd = new Random();
            var embed = new EmbedBuilder();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);
            var account = UserAccounts.UserAccounts.GetAccount(Context.User, Context.Guild);
            string prefix;
            if (server.Prefix == null) prefix = "Cat ";
            else prefix = server.Prefix;

            embed.WithTitle("Admin info for " + Context.User.Username);
            embed.WithDescription(":arrow_double_down:       :arrow_double_down: ");

            embed.WithColor(new Color(_rnd.Next(255), _rnd.Next(255), _rnd.Next(255)));
            await Context.Channel.SendMessageAsync("", false, embed);
            Console.WriteLine($"{DateTime.Now:G}" + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} || Used: TopUsers");
        }

        [Command("Creator")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Creator()
        {
            _rnd = new Random();
            var embed = new EmbedBuilder();

            embed.WithTitle("Info for " + Context.User.Username);
            embed.WithDescription(":arrow_double_down:       :arrow_double_down: \nMy creator is Brammys#5978");

            embed.WithColor(new Color(_rnd.Next(255), _rnd.Next(255), _rnd.Next(255)));
            await Context.Channel.SendMessageAsync("", false, embed);
            Console.WriteLine($"{DateTime.Now:G}" + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} || Used: TopUsers");
        }
    }
}
