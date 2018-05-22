using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Cat.Commands.ConsoleCmds;
using Cat.UserAccounts;
using Discord;
using Discord.WebSocket;
using Microsoft.Win32;

namespace Cat
{
    public class Program
    {
        DiscordSocketClient _client;
        private CommandHandler _handler;
        private string[] _args;

        static void Main(string[] args) => new Program().StartAsync(args).GetAwaiter().GetResult();

        public async Task StartAsync(string[] args)
        {
            _args = args;
            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
             Global.Client = _client;
            _client.Log += Log;
            _client.Ready += TimerLoop.WeeklyTimeConnectedWinner;
            _client.Ready += TimerLoop.CmdTimer;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Restart(_client, args);
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Global.log($"Data/Log/{DateTime.Now:dddd, MMMM d, yyyy}.txt", msg.Message);
            Console.WriteLine($"{DateTime.Now:G}" + " : " + msg.Message);
            return Task.CompletedTask;
        }

        public async Task Restart(DiscordSocketClient client, string[] args)
        {
            while (true)
            {
                await Task.Delay(2520000);
                Console.WriteLine("\r\nRestarting...");
                await _client.LogoutAsync();
                Main(_args);
            }
        }
    }
}
