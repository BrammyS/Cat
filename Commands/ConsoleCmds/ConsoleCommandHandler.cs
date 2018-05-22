using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Cat.Commands.ConsoleCmds
{
    public class ConsoleCommandHandler
    {
        public static void ConsoleInput(object sender, ElapsedEventArgs e)
        {
            while (true)
            {
                var info = Console.ReadLine();
                if (info != null && info.ToLower() == "update accounts")
                {
                    Console.WriteLine("\r\nAre you sure you want to update al accounts Y/N");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        Console.WriteLine("\r\nupdating...");
                        MiscCmds.updateAccounts();
                    }
                }
                if (info != null && info.ToLower() == "guilds") MiscCmds.Guilds();
            }
        }
    }
}
