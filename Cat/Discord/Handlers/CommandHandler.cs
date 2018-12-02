using System;
using System.Reflection;
using System.Threading.Tasks;
using Cat.Discord.Services;
using Cat.Discord.Services.Implementations;
using Cat.Services;
using Cat.Services.Implementations;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Cat.Discord.Handlers
{
    public class CommandHandler : ICommandHandler
    {
        private DiscordShardedClient _client;
        private IServiceProvider _services;
        private readonly IDiscordLogger _discordLogger;
        private readonly ILogger _logger;
        private CommandService _commandService;

        public CommandHandler(IDiscordLogger discordLogger, ILogger logger)
        {
            _discordLogger = discordLogger;
            _logger = logger;
        }

        public async Task InitializeAsync(DiscordShardedClient client)
        {
            _client = client;
            _commandService = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Info,
                DefaultRunMode = RunMode.Async
            });
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .AddScoped<IDiscordLogger, DiscordLogger>()
                .AddScoped<ILogger, Logger>()
                .BuildServiceProvider();
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly()).ConfigureAwait(false);
            _commandService.Log += CommandServiceLogAsync;
            _client.MessageReceived += HandleCommandEventAsync;
        }
        private async Task HandleCommandEventAsync(SocketMessage message)
        {
            if (!(message is SocketUserMessage msg)) return;
            await HandleCommandAsync(msg).ConfigureAwait(false);
        }

        private async Task HandleCommandAsync(SocketUserMessage msg)
        {
            try
            {
                if (msg.Author.IsBot) return;
                var context = new ShardedCommandContext(_client, msg);
                var argPos = 0;
                if (context.Message.HasStringPrefix("?", ref argPos) || 
                    context.Message.HasMentionPrefix(_client.CurrentUser, ref argPos))
                {
                    var searchResult = _commandService.Search(context, argPos);
                    if (searchResult.Commands == null || searchResult.Commands.Count == 0) return;
                    var result = await _commandService.ExecuteAsync(context, argPos, _services).ConfigureAwait(false);
                    Console.WriteLine(result.ErrorReason);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task CommandServiceLogAsync(LogMessage logMessage)
        {
            if (logMessage.Exception is CommandException commandException)
            {
                await commandException.Context.Channel.SendMessageAsync(commandException.Message).ConfigureAwait(false);
            }
            _logger.Log("CommandService/ErrorDetails", $"Message: {logMessage.Message}, exception: {logMessage.Exception}, source: {logMessage.Source}");
        }
    }
}
