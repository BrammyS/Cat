using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cat.UserAccounts;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Cat.Commands.TestCmd
{
    public class MiscTest : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = { /*Brammys*/308707063993860116 };
        private Random rnd;

        [Command("UpdateAccounts")]
        public async Task test()
        {
            var GuildUser = await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id);
            if (admins.Contains(GuildUser.Id))
            {
                var embed = new EmbedBuilder();
                rnd = new Random();
                
            }
            else
            {
                //error message
                await Context.Channel.SendMessageAsync(":no_entry: You shall not use this command :no_entry: ");
                Console.WriteLine($"{DateTime.Now:G}" + $" Server: {Context.Guild} || Channel: {Context.Channel}");
            }
        }

        [Command("day")]
        public async Task LoadAccounts()
        {
            TimeSpan interval = new TimeSpan(7);
            Console.WriteLine(DateTime.Now.DayOfWeek);

            Console.WriteLine(DateTime.Now.DayOfWeek);
            Console.WriteLine(DateTime.Now.DayOfWeek);
            try
            {
                await Context.Channel.SendMessageAsync(DateTime.Now.DayOfWeek.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [Command("addxp")]
        public async Task addxp(uint xp)
        {

            var GuildUser = await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id);
            if (admins.Contains(GuildUser.Id))
            {
                var embed = new EmbedBuilder();
                rnd = new Random();
                var account = UserAccounts.UserAccounts.GetAccount(Context.User, Context.Guild);
                UserAccounts.UserLeveling.AddXp(Context.User, Context.Guild, xp);
                UserAccounts.UserAccounts.SaveAccounts(Context.User, Context.Guild);

                embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                await Context.Channel.SendMessageAsync($"added {xp}xp");
            }
            else
            {
                //error message
                await Context.Channel.SendMessageAsync(":no_entry: You shall not use this command :no_entry: ");
                Console.WriteLine($"{DateTime.Now:G}" + $" Server: {Context.Guild} || Channel: {Context.Channel}");
            }
        }
    }
}
