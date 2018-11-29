using System;
using System.IO;
using System.Threading.Tasks;
using Cat.Services;
using Discord;
using Discord.Commands;

namespace Cat.Discord.Services.Implementations
{
    public class DiscordLogger : IDiscordLogger
    {
        private readonly ILogger _logger;

        public DiscordLogger(ILogger logger)
        {
            _logger = logger;
        }

        public Task Log(LogMessage logMsg)
        {
            _logger.Log(logMsg.Message);
            return Task.CompletedTask;
        }

        public void Log(string folder, IResult result, ICommandContext context)
        {
            Task.Run(() =>
            {
                var filePath = $"Logs/{folder}/{DateTime.Now:MMMM, yyyy}";
                if (!File.Exists(filePath)) Directory.CreateDirectory(filePath);

                filePath += $"/{DateTime.Now:dddd, MMMM d, yyyy}.txt";
                using (var file = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    using (var sw = new StreamWriter(file))
                    {
                        sw.WriteLine($"{DateTime.Now:T} : {result.ErrorReason}");
                        sw.WriteLine($"{DateTime.Now:T} : {context.Message}");
                        sw.WriteLine($"{DateTime.Now:T} : User: {context.User.Username}");
                        sw.WriteLine($"{DateTime.Now:T} : Guild: {context.Guild.Name} Id: {context.Guild.Id}");
                        sw.WriteLine("===========================================================");
                    }
                    file.Flush();
                    file.Close();
                }
            });
        }
    }
}