using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Cat.UserAccounts
{
    public class UserLeveling
    {

        public static void AddXp(SocketUser user, SocketGuild guild, uint xp)
        {
            var account = UserAccounts.GetAccount(user, guild);
            var server = ServerAccounts.ServerAccounts.GetAccount(guild);
            bool levelUp = false;
            ulong nextLevel = account.Level * 25;
            while (account.XP >= nextLevel)
            {
                account.XP -= account.Level * 25;
                account.Level += 1;
                nextLevel = account.Level * 25;
                levelUp = true;
            }
            account.XP += xp;
            if (server.LevelupMsgBool)
            {
                if (levelUp)
                {
                    string txt = "";
                    if (!string.IsNullOrEmpty(server.Prefix))
                    {
                        txt = $"\nUse `{server.Prefix}Level` for more info";
                    }
                    else
                    {
                        txt = "\nUse `Cat Level` for more info";
                    }
                    try
                    {
                        if (server.LevelupMsgChannel != 0) guild.GetTextChannel(server.LevelupMsgChannel).SendMessageAsync($"{user.Mention} just leveled up to lvl: **{account.Level}** :tada:" + txt);
                        else guild.DefaultChannel.SendMessageAsync($"{user.Mention} just leveled up to lvl: **{account.Level}** :tada:" + txt);
                    }
                    catch (Exception e) {Console.WriteLine(e);}
                    try
                    {
                        Global.Client.GetGuild(403577303784882186).GetTextChannel(423830437035638804)
                            .SendMessageAsync($"Server: {guild.Name} id: {guild.Id} || {user} just leveled up to lvl: **{account.Level}**");
                    }
                    catch (Exception e) { Console.WriteLine(e); }
                }
            }
            UserAccounts.SaveAccounts(user, guild);
        }

        public static void LastActivity(SocketUser user, SocketGuild guild)
        {
            var account = UserAccounts.GetAccount(user, guild);
            account.TimeConnected = DateTime.Now;
            UserAccounts.SaveAccounts(user, guild);
        }

        public static void TotalTimeConntected(SocketUser user, SocketGuild guild)
        {
            var account = UserAccounts.GetAccount(user, guild);
            TimeSpan timeDif = DateTime.Now.Subtract(account.TimeConnected);
            account.TotalTimeConnected += ulong.Parse(Math.Round(timeDif.TotalMinutes, 0, MidpointRounding.ToEven).ToString());
            account.TimeConnectedWeek += ulong.Parse(Math.Round(timeDif.TotalMinutes, 0, MidpointRounding.ToEven).ToString());
            account.TimeConnectedMonth += ulong.Parse(Math.Round(timeDif.TotalMinutes, 0, MidpointRounding.ToEven).ToString());
            account.TimeConnectedYear += ulong.Parse(Math.Round(timeDif.TotalMinutes, 0, MidpointRounding.ToEven).ToString());
            UserAccounts.SaveAccounts(user, guild);
        }

        public static void LastEmoteSend(SocketUser user, SocketGuild guild)
        {
            var account = UserAccounts.GetAccount(user, guild);
            account.LastEmote = DateTime.Now;
            UserAccounts.SaveAccounts(user, guild);
        }

        public static void LastCommandsUsed(SocketUser user, SocketGuild guild)
        {
            var account = UserAccounts.GetAccount(user, guild);
            account.LastCommandUsed = DateTime.Now;
            UserAccounts.SaveAccounts(user, guild);
        }

        public static void TimesTimedOut(SocketUser user, SocketGuild guild)
        {
            var account = UserAccounts.GetAccount(user, guild);
            account.TimesTimedOut++;
            UserAccounts.SaveAccounts(user, guild);
        }
    }
}
