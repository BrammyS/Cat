using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Cat
{
    internal static class Global
    {
        internal static DiscordSocketClient Client { get; set; }

        internal static void log(string filePath, string text)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine($"{DateTime.Now:G}" + $" : " + text);
            }
        }
    }
}
