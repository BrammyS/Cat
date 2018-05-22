using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Cat.UserAccounts;
using Discord;
using Discord.Commands;

namespace Cat.Commands
{
    public class UsefullMiscCmds : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = { /*Brammys*/308707063993860116 };
        private Random _rnd;

        [Command("level")]
        public async Task ShowLevel()
        {
            _rnd = new Random();
            var embed = new EmbedBuilder();
            var account = UserAccounts.UserAccounts.GetAccount(Context.User, Context.Guild);
            ulong NextLevel = account.Level * 25;
            long xpNeeded = (long)(NextLevel - account.XP);

            embed.WithTitle("Info for " + Context.User.Username);
            embed.WithDescription(":arrow_double_down:       :arrow_double_down: ");
            embed.AddInlineField("Level", account.Level);
            embed.AddInlineField("XP", account.XP);
            embed.AddInlineField("Next lvl", $"Next level in **{xpNeeded}** XP");
            embed.AddInlineField("Total time connected (In minutes)", account.TotalTimeConnected);
            embed.WithColor(new Color(_rnd.Next(255), _rnd.Next(255), _rnd.Next(255)));

            await Context.Channel.SendMessageAsync("", false, embed);
            Console.WriteLine($"{DateTime.Now:G}" + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} || Used: level");
        }

        [Command("TopUsers")]
        public async Task TopUsers()
        {
            _rnd = new Random();
            var embed = new EmbedBuilder();
            embed.WithTitle("Info for " + Context.User.Username);
            ulong a = 0, b = 0, c = 0, d = 0;
            ulong aLevel = 0, bLevel = 0, cLevel = 0, dLevel = 0;
            string aLevelName = "Empty", bLevelName = "Empty", cLevelName = "Empty";
            string aName = "Empty", bName = "Empty", cName = "Empty";
            foreach (var user in Global.Client.GetGuild(Context.Guild.Id).Users)
            {
                if (Datastorage.LoadUserAccounts($"Data/UserData/{Context.Guild.Id}/{user.Id}.json") != null)
                {
                    try
                    {
                        List<UserAccount> accounts;
                        accounts = Datastorage.LoadUserAccounts($"Data/UserData/{Context.Guild.Id}/{user.Id}.json").ToList();

                        embed.WithTitle("Info for " + Context.User.Username);
                        foreach (var account in accounts)
                        {
                            dLevel = account.Level;
                            if (dLevel > cLevel)
                            {
                                cLevel = dLevel;
                                cLevelName = account.Username;
                                if (cLevel > bLevel)
                                {
                                    cLevel = bLevel;
                                    bLevel = dLevel;
                                    cLevelName = bLevelName;
                                    bLevelName = account.Username;
                                    if (bLevel > aLevel)
                                    {
                                        bLevel = aLevel;
                                        aLevel = dLevel;
                                        bLevelName = aLevelName;
                                        aLevelName = account.Username;
                                    }
                                }
                            }
                        }
                        foreach (var account in accounts)
                        {
                            d = account.TotalTimeConnected;
                            if (d > c)
                            {
                                c = d;
                                cName = account.Username;
                                if (c > b)
                                {
                                    c = b;
                                    b = d;
                                    cName = bName;
                                    bName = account.Username;
                                    if (b > a)
                                    {
                                        b = a;
                                        a = d;
                                        bName = aName;
                                        aName = account.Username;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
            //top 3 levels
            embed.AddField(":arrow_double_down:", "Top 3 users with the highest level.");
            embed.AddInlineField("First place", $"{aLevelName} with level {aLevel}.");
            embed.AddInlineField("Second place", $"{bLevelName} with level {bLevel}.");
            embed.AddInlineField("Third place", $"{cLevelName} with level {cLevel}.\n");
            //top 3 total time connected
            embed.AddField(":arrow_double_down:", "Top 3 users with the most time connected.");
            embed.AddInlineField("First place", $"{aName} with {a} minutes.");
            embed.AddInlineField("Second place", $"{bName} with {b} minutes.");
            embed.AddInlineField("Third place", $"{cName} with {c} minutes.");

            embed.WithColor(new Color(_rnd.Next(255), _rnd.Next(255), _rnd.Next(255)));
            await Context.Channel.SendMessageAsync("", false, embed);
            Console.WriteLine($"{DateTime.Now:G}" + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} || Used: level");
        }
    }
}
