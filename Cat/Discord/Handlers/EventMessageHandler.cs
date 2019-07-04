using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Cat.Discord.Handlers
{

    public class EventMessageHandler
    {
        private DiscordShardedClient _client;


        public void Initialize(DiscordShardedClient client)
        {
            _client = client;
            _client.GuildMemberUpdated += PuddingAdded;
        }

        private Task PuddingAdded(SocketGuildUser oldUser, SocketGuildUser newUser)
        {
            try
            {
                if (newUser.Roles.Select(x => x.Id).ToList().Contains(Constants.RoleIds.PuddingId) && !oldUser.Roles.Select(x => x.Id).ToList().Contains(Constants.RoleIds.PuddingId))
                {
                    Console.WriteLine("New pudding");
                    var message = $"{newUser.Mention} welcome to the puddings!\n" +
                                  $"This is our own little safe space for the women in LoS, if you have any questions or anything, feel free to message {_client.GetUser(163341531514667008).Mention} :purple_heart:";
                    _client.GetGuild(Constants.GuildIds.Los).GetTextChannel(Constants.ChannelIds.TextChannelIds.Pudding).SendMessageAsync(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Task.CompletedTask;
        }
    }
}
