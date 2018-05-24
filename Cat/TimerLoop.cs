using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Cat.BotData;
using Cat.Commands.ConsoleCmds;
using Cat.UserAccounts;
using Discord;
using static System.TimeSpan;

namespace Cat
{
    internal static class TimerLoop
    {
        private static Timer _weeklyTimer;
        private static Timer _cmdTimer;

        //All in miliseconds
        private static long day = 86400000;

        private static Random _rnd;
        private static bool _timer = true;
        internal static Task WeeklyTimeConnectedWinner()
        {

            _weeklyTimer = new Timer
            {
                Interval = 28800000,
                AutoReset = true,
                Enabled = true
            };
            _weeklyTimer.Elapsed += WeeklyTimerTicked;
            return Task.CompletedTask;
        }

        internal static Task CmdTimer()
        {
            _cmdTimer = new Timer
            {
                Interval = 5,
                AutoReset = false,
                Enabled = true
            };
            _cmdTimer.Elapsed += ConsoleCommandHandler.ConsoleInput;
            return Task.CompletedTask;
        }

        private static void WeeklyTimerTicked(object sender, ElapsedEventArgs e)
        {
            
            if (Global.Client == null)
            {
                Console.WriteLine("Client was not set");
                return;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && _timer)
            {
                foreach (var guildId in Bot.GetAccount().Guilds)
                {
                    var guild = Global.Client.GetGuild(guildId);
                    var server = ServerAccounts.ServerAccounts.GetAccount(guild);
                    if (server.WeeklyTopBool && server.LevelupMsgBool)
                    {
                        _rnd = new Random();
                        var embed = new EmbedBuilder();
                        string[] filePaths =
                        {
                        "LessThen050000000000000000", "LessThen075000000000000000", "LessThen100000000000000000", "LessThen120000000000000000", "LessThen140000000000000000", "LessThen160000000000000000", "LessThen180000000000000000",
                        "LessThen200000000000000000", "LessThen220000000000000000", "LessThen240000000000000000", "LessThen260000000000000000", "LessThen280000000000000000", "LessThen300000000000000000", "LessThen320000000000000000",
                        "LessThen340000000000000000", "LessThen360000000000000000", "LessThen380000000000000000", "LessThen400000000000000000", "LessThen420000000000000000", "LessThen440000000000000000", "LessThen460000000000000000",
                        "LessThen480000000000000000", "LessThen500000000000000000", "LessThen520000000000000000", "LessThen540000000000000000", "LessThen560000000000000000", "LessThen580000000000000000", "LessThen600000000000000000",
                        "LessThen620000000000000000", "LessThen640000000000000000", "LessThen660000000000000000", "LessThen680000000000000000", "LessThen700000000000000000"
                        };
                        ulong a = 0, b = 0, c = 0, d = 0;
                        string aName = "Empty", bName = "Empty", cName = "Empty";
                        foreach (var filePath in filePaths)
                        {
                            if (Datastorage.LoadUserAccounts($"Data/UserData/{guild.Id}/{filePath}.json") != null)
                            {
                                try
                                {
                                    var accounts = Datastorage.LoadUserAccounts($"Data/UserData/{guild.Id}/{filePath}.json").ToList();
                                    foreach (var account in accounts)
                                    {
                                        d = account.TimeConnectedWeek;
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
                                        Console.WriteLine("Weekly time connected = 0");
                                        account.TimeConnectedWeek = 0;
                                       Datastorage.SaveUserAccounts(accounts, $"Data/UserData/{guild.Id}/{filePath}.json");
                                    }
                                }
                                catch (Exception t)
                                {
                                    Console.WriteLine(t);
                                    throw;
                                }
                            }
                        }
                        //top 3 total time connected
                        embed.AddField(":arrow_double_down:", "Top 3 most time connected of this week. ");
                        embed.AddInlineField("First place", $"{aName} with {a} minutes.");
                        embed.AddInlineField("Second place", $"{bName} with {b} minutes.");
                        embed.AddInlineField("Third place", $"{cName} with {c} minutes.");
                        embed.WithColor(new Color(_rnd.Next(255), _rnd.Next(255), _rnd.Next(255)));
                        guild.GetTextChannel(server.LevelupMsgChannel).SendMessageAsync("", false, embed);
                        _timer = false;
                    }
                }
            }

            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                _timer = true;
                Console.WriteLine("test");
            }
        }
    }
}
