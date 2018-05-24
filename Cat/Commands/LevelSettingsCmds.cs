using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cat.BotData;
using Discord;
using Discord.Commands;

namespace Cat.Commands
{
    public class LevelSettingsCmds : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = { /*Brammys*/308707063993860116 };
        private Random rnd;

        [Command("levelDebug")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task levelDebug()
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            embed.AddInlineField("Level up message:", server.LevelupMsgBool);
            embed.AddInlineField("Level up message channelId:", server.LevelupMsgChannel);

            embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("levelupmessage")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task JoinDm(bool setting)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.LevelupMsgBool = setting;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            embed.WithDescription($"Done!  :thumbsup: ");

            embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("levelupchannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task levelupchannel(ulong setting)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);

            server.LevelupMsgChannel = setting;
            ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
            embed.WithDescription($"Done!  :thumbsup: ");

            embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("WeeklyTop")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task WeeklyTop(bool setting)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);
            var bot = Bot.GetAccount();
            try
            {
                server.WeeklyTopBool = setting;
                ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
                embed.WithDescription($"Done!  :thumbsup: ");
                embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                await Context.Channel.SendMessageAsync("", false, embed);

                ulong i = 0;
                foreach (var serverId in bot.Guilds)
                {
                    Console.WriteLine(serverId);
                    if (serverId == Context.Guild.Id)
                    {
                        Console.WriteLine("server already added");
                        return;
                    }
                    if (serverId == 0)
                    {
                        bot.Guilds[i] = Context.Guild.Id;
                        Bot.SaveAccounts();
                        return;
                    }
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [Command("MonthlyTop")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task MonthlyTop(bool setting)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);
            var bot = Bot.GetAccount();
            try
            {
                server.MonthlyTopBool = setting;
                ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
                embed.WithDescription($"Done!  :thumbsup: ");
                embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                await Context.Channel.SendMessageAsync("", false, embed);

                ulong i = 0;
                foreach (var serverId in bot.Guilds)
                {
                    Console.WriteLine(serverId);
                    if (serverId == Context.Guild.Id)
                    {
                        Console.WriteLine("server already added");
                        return;
                    }
                    if (serverId == 0)
                    {
                        bot.Guilds[i] = Context.Guild.Id;
                        Bot.SaveAccounts();
                        return;
                    }
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("YearlyTop")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task YearlyTop(bool setting)
        {
            var embed = new EmbedBuilder();
            rnd = new Random();
            var server = ServerAccounts.ServerAccounts.GetAccount(Context.Guild);
            var bot = Bot.GetAccount();
            try
            {
                server.YearlyTopBool = setting;
                ServerAccounts.ServerAccounts.SaveAccounts(Context.Guild);
                embed.WithDescription($"Done!  :thumbsup: ");
                embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                await Context.Channel.SendMessageAsync("", false, embed);

                ulong i = 0;
                foreach (var serverId in bot.Guilds)
                {
                    Console.WriteLine(serverId);
                    if (serverId == Context.Guild.Id)
                    {
                        Console.WriteLine("server already added");
                        return;
                    }
                    if (serverId == 0)
                    {
                        bot.Guilds[i] = Context.Guild.Id;
                        Bot.SaveAccounts();
                        return;
                    }
                    i++;
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
