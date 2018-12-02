using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Cat.Discord.Interfaces;
using Cat.Persistence.Interfaces.UnitOfWork;
using Cat.Services;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore.Internal;

namespace Cat.Discord.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        private DiscordShardedClient _client;
        private readonly ILogger _logger;

        public MessageHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task InitializeAsync(DiscordShardedClient client)
        {
            _client = client;

            _client.MessageReceived += MessageReceived;
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (!(message is SocketUserMessage msg)) return;
            await MessageReceivedAsync(msg);
        }

        private async Task MessageReceivedAsync(SocketUserMessage message)
        {
            var context = new ShardedCommandContext(_client, message);
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var user = await unitOfWork.UserInfos.GetUserInfoAsync(context.Guild.Id, context.User.Id);
                Console.WriteLine(DateTime.Now.Subtract(user.LastMessageSend).TotalSeconds);
                if (!(DateTime.Now.Subtract(user.LastMessageSend).TotalSeconds > 4)) return;
                user.Xp += 2;
                user.LastMessageSend = DateTime.Now;
                Console.WriteLine(user.Xp);
                await unitOfWork.SaveAsync();
            }
        }
    }
}