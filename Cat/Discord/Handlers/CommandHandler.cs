using System;
using System.Reflection;
using System.Threading.Tasks;
using Cat.Discord.Services;
using Cat.Discord.Services.Implementations;
using Cat.Persistence.EntityFrameworkCore.Models;
using Cat.Persistence.EntityFrameworkCore.Repositories;
using Cat.Persistence.EntityFrameworkCore.UnitOfWork;
using Cat.Persistence.Interfaces.Repositories;
using Cat.Persistence.Interfaces.UnitOfWork;
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
        private readonly ILogger _logger;
        private readonly IExpService _expService;
        private CommandService _commandService;

        public CommandHandler(ILogger logger, IExpService expService)
        {
            _logger = logger;
            _expService = expService;
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
                .AddDbContext<CatContext>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IServerRepository, ServerRepository>()
                .AddScoped<ILogsRepository, LogsRepository>()
                .AddScoped<IDiscordLogger, DiscordLogger>()
                .AddScoped<ILogger, Logger>()
                .BuildServiceProvider();
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services).ConfigureAwait(false);
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
                    using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
                    {
                        var searchResult = _commandService.Search(context, argPos);
                        if (searchResult.Commands == null || searchResult.Commands.Count == 0) return;
                        var result = await _commandService.ExecuteAsync(context, argPos, _services).ConfigureAwait(false);
                        var user = await unitOfWork.Users.GetOrAddUserInfoAsync(context.Guild.Id, context.User.Id, context.User.Username).ConfigureAwait(false);
                        if (!(DateTime.Now.Subtract(user.LastMessageSend).TotalSeconds > 4)) return;
                        await _expService.GiveXp(3, user, context.Guild, context.User, unitOfWork).ConfigureAwait(false);
                        user.LastMessageSend = DateTime.Now;
                        user.MessagesSend++;
                        await unitOfWork.SaveAsync().ConfigureAwait(false);
                        Console.WriteLine(result.ErrorReason);
                    }
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
