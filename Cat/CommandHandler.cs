using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Cat.UserAccounts;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Cat
{
    public class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _service;
        private Random rnd;
        private int _memberCount = 0;
        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync; //handling a command
            _client.GuildAvailable += ConnectedToGuild; //adding guild member count to total member count
            _client.Connected += SetGame; //Setting the game that the bot is playing
            _client.UserJoined += JoinDm; //Sending a DM when a user joined the server
            _client.UserVoiceStateUpdated += UserJoinedOrLeftChannel;// Adding xp and total time connected to the user for staying in a voice channel
            _client.ReactionAdded += ReactionAdded; //adding xp for adding a reaction to  message
            _client.MessageReceived += userSendMessage; //Added xp to the user for sending a message

            // TO DO_client.JoinedGuild
        }

        private Task ConnectedToGuild(SocketGuild guild)
        {
            _memberCount += guild.MemberCount;
            return Task.CompletedTask;
        }

        public async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            var server = ServerAccounts.ServerAccounts.GetAccount(context.Guild);
            int argPos = 0;
            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos)|| msg.HasMentionPrefix(_client.CurrentUser, ref argPos) || msg.HasStringPrefix(server.Prefix, ref argPos) && !context.User.IsBot)
            {
                var account = UserAccounts.UserAccounts.GetAccount(context.User, context.Guild);
                if (DateTime.Now.Subtract(account.LastCommandUsed).TotalSeconds > 6)
                {
                    if(DateTime.Now.Subtract(account.LastCommandUsed).TotalSeconds > 180) account.TimesTimedOut = 0;
                    var result = await _service.ExecuteAsync(context, argPos);
                    if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    {
                        if (result.ToString() == "BadArgCount: The input text has too many parameters.")
                        {
                            await context.Channel.SendMessageAsync(":no_entry_sign: The input has to many parameters");
                        }
                        else if (result.ToString() == "BadArgCount: The input text has too few parameters.")
                        {
                            await context.Channel.SendMessageAsync(
                                ":no_entry_sign: The input text has too few parameters.");
                        }
                        else if (result.ToString() == "UnmetPrecondition: User requires guild permission Administrator")
                        {
                            await context.Channel.SendMessageAsync(
                                ":no_entry_sign: You need admin permissions for this command.");
                        }
                        Console.WriteLine(result.ErrorReason);
                    }
                    if(result.IsSuccess)UserLeveling.LastCommandsUsed(context.User, context.Guild);
                }
                else
                {
                    if (account.TimesTimedOut == 15)
                    {
                        account.LastCommandUsed = account.LastCommandUsed.AddMinutes(15);
                        account.TimesTimedOut = 0;
                        await context.Channel.SendMessageAsync($"{context.User.Mention} you got timed out for 15 min\ntrying to use my commands to often.");
                    }
                    else if(DateTime.Now.Subtract(account.LastCommandUsed).TotalSeconds > 0)
                    {
                        UserLeveling.TimesTimedOut(context.User, context.Guild);
                        await context.Channel.SendMessageAsync($"pls wait {6 - Math.Round(DateTime.Now.Subtract(account.LastCommandUsed).TotalSeconds, 0)}s before using another commands.");
                    }
                    
                }
                UserAccounts.UserAccounts.SaveAccounts(context.User, context.Guild);
            }
        }

        public Task userSendMessage(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            var context = new SocketCommandContext(_client, msg);
            //adding xp for sending message 
            if(!context.User.IsBot) UserLeveling.AddXp(context.User, context.Guild, 3);
            return Task.CompletedTask;
        }

        public Task ReactionAdded(Cacheable<IUserMessage, ulong> userMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            SocketUser user = Global.Client.GetUser(reaction.UserId);
            if (!user.IsBot)
            {
                if (reaction.Channel is SocketGuildChannel chnl)
                {
                    var guild = chnl.Guild;
                    var account = UserAccounts.UserAccounts.GetAccount(user, guild);
                    if (DateTime.Now.Subtract(account.LastEmote).TotalSeconds > 5)
                    {
                        UserLeveling.AddXp(user, guild, 9);
                        UserLeveling.LastEmoteSend(user, guild);
                    }
                }
            }
            return Task.CompletedTask;
        }

        public async Task SetGame()
        {
            while (true)
            {
                if (_memberCount != 0)
                {
                    await _client.SetGameAsync($"Type 'Cat Help' | {_memberCount} users");
                    await Task.Delay(60000);
                }
                await _client.SetGameAsync($"Type 'Cat Help' | {Global.Client.Guilds.Count} servers");
                await Task.Delay(60000);
            }
        }

        public Task UserJoinedOrLeftChannel(SocketUser user, SocketVoiceState voiceState1, SocketVoiceState voiceState2)
        {
            //int usercount = voiceState1.VoiceChannel.Users.Count;
            SocketGuild guild;
            try
            {
                guild = voiceState1.VoiceChannel.Guild;
            }
            catch (Exception e)
            {
                guild = voiceState2.VoiceChannel.Guild;
            }
            if (!user.IsBot)
            {
                var account = UserAccounts.UserAccounts.GetAccount(user, guild);
                var dt = DateTime.Now;
                var voiceState1String = voiceState1.ToString();
                var timeDif = dt.Subtract(account.TimeConnected);

                string path = $"Data/ServerData/{guild.Id}/{DateTime.Now:dddd, MMMM d, yyyy}.txt";
                Global.log(path, $"{user} connected to {voiceState2} from {voiceState1}");    
                if (voiceState1String != "Unknown" && voiceState1String != "AFK")
                {
                    int i = 0;
                    double timeDiffMinutes = timeDif.TotalMinutes;
                    uint xp = 0;
                    while (timeDiffMinutes >= 3)
                    {
                        if (timeDiffMinutes > 960)
                        {
                            UserLeveling.LastActivity(user, guild); //resetting  LastActivity
                            timeDiffMinutes = 20;                        }
                        xp++;
                        timeDiffMinutes -= 3;
                        i++;
                    }
                    UserLeveling.AddXp(user, guild, xp); //adding XP
                    UserLeveling.TotalTimeConntected(user, guild); //adding total minutes to account
                    if (timeDif.TotalMinutes > 3)
                    {
                        Global.log(path, $"{user} gained {i * 1} XP by staying in {voiceState1} for {timeDif.TotalMinutes}");
                    }
                }
                UserLeveling.LastActivity(user, guild); //setting new LastActivity
            }
            return Task.CompletedTask;
        }

        public async Task JoinDm(SocketGuildUser guildUser)
        {
            var s = ServerAccounts.ServerAccounts.GetAccount(guildUser.Guild);
            if (s.JoinDmBool)
            {
                var embed = new EmbedBuilder();
                rnd = new Random();
                Console.WriteLine($"{DateTime.Now:G}" + $" : {guildUser.Username} joined {guildUser.Guild.Name}");
                if (s.JoinDmTitleString != "") embed.WithTitle(s.JoinDmTitleString);
                if (s.JoinDmImageUrlString != "") embed.WithImageUrl(s.JoinDmImageUrlString);
                if (s.JoinDmTumbnailImageUrlString != "") embed.WithThumbnailUrl(s.JoinDmTumbnailImageUrlString);
                if (s.JoinDmDescriptionString != "") embed.WithDescription(s.JoinDmDescriptionString);
                if (s.JoinDmField1TitleString != "" && s.JoinDmTitleString != "") embed.AddField(s.JoinDmField1TitleString, s.JoinDmField1String);
                if (s.JoinDmField2TitleString != "" && s.JoinDmField2String != "") embed.AddField(s.JoinDmField2TitleString, s.JoinDmField2String);
                if (s.JoinDmField3TitleString != "" && s.JoinDmField3String != "") embed.AddField(s.JoinDmField3TitleString, s.JoinDmField3String);
                if (s.JoinDmField4TitleString != "" && s.JoinDmField4String != "") embed.AddField(s.JoinDmField4TitleString, s.JoinDmField4String);
                if (s.JoinDmField5TitleString != "" && s.JoinDmField5String != "") embed.AddField(s.JoinDmField5TitleString, s.JoinDmField5String);
                if (s.JoinDmField6TitleString != "" && s.JoinDmField6String != "") embed.AddField(s.JoinDmField6TitleString, s.JoinDmField6String);
                if (s.JoinDmInlineField1TitleString != "" && s.JoinDmInlineField1String != "") embed.AddInlineField(s.JoinDmInlineField1TitleString, s.JoinDmInlineField1String);
                if (s.JoinDmInlineField2TitleString != "" && s.JoinDmInlineField2String != "") embed.AddInlineField(s.JoinDmInlineField2TitleString, s.JoinDmInlineField2String);
                if (s.JoinDmInlineField3TitleString != "" && s.JoinDmInlineField3String != "") embed.AddInlineField(s.JoinDmInlineField3TitleString, s.JoinDmInlineField3String);
                if (s.JoinDmInlineField4TitleString != "" && s.JoinDmInlineField4String != "") embed.AddInlineField(s.JoinDmInlineField4TitleString, s.JoinDmInlineField4String);
                if (s.JoinDmFooterString != "") embed.WithFooter(s.JoinDmFooterString);

                embed.WithCurrentTimestamp();
                embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                await guildUser.SendMessageAsync("", false, embed);
            }
        }
    }
}
