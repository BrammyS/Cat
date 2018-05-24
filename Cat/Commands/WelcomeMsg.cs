using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Cat.Commands
{
    public class WelcomeMsg : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = { /*Brammys*/308707063993860116 };
        private Random rnd;

        [Command("welcomeTest")]
        public async Task welcomeTest()
        {
            var GuildUser = await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id);
            if (admins.Contains(GuildUser.Id))
            {
                var embed = new EmbedBuilder();
                rnd = new Random();
                

                embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
            {
                //error message
                await Context.Channel.SendMessageAsync(":no_entry: You shall not use this command :no_entry: ");
                Console.WriteLine(String.Format("{0:G}") + $" Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} tried to use ?AddGame ");
            }
        }
    }
}
