using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cat.UserAccounts;
using Discord;
using Discord.WebSocket;

namespace Cat.Commands.ConsoleCmds
{
    public class MiscCmds
    {
        public static void updateAccounts()
        {
            string[] filePaths =
                {
                    "LessThen050000000000000000", "LessThen075000000000000000", "LessThen100000000000000000","LessThen120000000000000000", "LessThen140000000000000000", "LessThen160000000000000000","LessThen180000000000000000",
                    "LessThen200000000000000000", "LessThen220000000000000000", "LessThen240000000000000000","LessThen260000000000000000", "LessThen280000000000000000", "LessThen300000000000000000","LessThen320000000000000000",
                    "LessThen340000000000000000", "LessThen360000000000000000", "LessThen380000000000000000","LessThen400000000000000000", "LessThen420000000000000000", "LessThen440000000000000000","LessThen460000000000000000",
                    "LessThen480000000000000000", "LessThen500000000000000000", "LessThen520000000000000000","LessThen540000000000000000", "LessThen560000000000000000", "LessThen580000000000000000","LessThen600000000000000000",
                    "LessThen620000000000000000", "LessThen640000000000000000", "LessThen660000000000000000","LessThen680000000000000000", "LessThen700000000000000000"
                };
            foreach (var guild in Global.Client.Guilds)
            {
                Console.WriteLine(guild.Name + "-==================================================-");
                foreach (var filePath in filePaths)
                {
                    Console.WriteLine(filePath + "-----------------------------------------------");
                    if (Datastorage.LoadUserAccounts($"Data/UserData/old/{guild.Id}/{filePath}.json") != null)
                    {
                        var accounts = Datastorage.LoadUserAccounts($"Data/UserData/old/{guild.Id}/{filePath}.json");
                        foreach (var account in accounts)
                        {
                            IEnumerable<UserAccount> newAccounts = new[] { account };
                            Console.WriteLine(account.Username + "----");
                            Datastorage.SaveUserAccounts(newAccounts, $"Data/UserData/{guild.Id}/{account.ID}.json");
                        }
                    }
                }
            }
        }

        public static async void Guilds()
        {
            Console.WriteLine("Select the guild:");
            var guild = GetSelectedGuild(Global.Client.Guilds);
            var channel = GetSelectedChannel(guild.TextChannels);
            var msg = string.Empty;
            while (msg.Trim() == string.Empty)
            {
                Console.WriteLine("Your message:");
                msg = Console.ReadLine();
            }
            await channel.SendMessageAsync(msg);
        }

        private static SocketGuild GetSelectedGuild(IEnumerable<SocketGuild> guilds)
        {
            var socketGuilds = guilds.ToList();
            int maxIndex = socketGuilds.Count() - 1;
            for (int i = 0; i <= maxIndex; i++)
            {
                Console.WriteLine($"{i} || {socketGuilds.ToList()[i].Name}");
            }
            var index = -1;
            while (index < 0 || index > maxIndex)
            {
                var success = int.TryParse(Console.ReadLine(), out index);
                if (!success)
                {
                    Console.WriteLine("That was an invalid index.");
                    index = -1;
                }
            }
            return socketGuilds[index];
        }

        private static SocketTextChannel GetSelectedChannel(IEnumerable<SocketTextChannel> channels)
        {
            var socketTextChannel = channels.ToList();
            var maxIndex = socketTextChannel.Count - 1;
            for (int i = 0; i <= maxIndex; i++)
            {
                Console.WriteLine($"{i} || {socketTextChannel.ToList()[i].Name}");
            }
            var index = -1;
            while (index < 0 || index > maxIndex)
            {
                var success = int.TryParse(Console.ReadLine(), out index);
                if (!success)
                {
                    Console.WriteLine("That was an invalid index.");
                    index = -1;
                }
            }
            return socketTextChannel[index];
        }
    }
}
