using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat.Persistence.Domain.Tables;
using Cat.Persistence.Interfaces.UnitOfWork;
using Cat.Persistence.JsonStorage.UserAccounts;
using Discord.WebSocket;

namespace Cat.Persistence.JsonStorage
{
    public class DataLoader
    {
        public async Task SaveDataToDatabase(DiscordSocketClient client, IUnitOfWork unitOfWork)
        {
            try
            {
                var newUserAccounts = new List<User>();
                var userInfos = new List<UserInfo>();
                var guild = client.GetGuild(403577303784882186);
                Console.WriteLine($"guild: {guild.Name}");
                var oldUserAccounts = new List<UserAccount>();
                oldUserAccounts.AddRange(guild.Users.Select(user =>
                {
                    Console.WriteLine($"Loading: {user.Username}");
                    return UserAccounts.UserAccounts.GetAccount(user, guild);
                }));
                foreach (var oldUserAccount in oldUserAccounts)
                {
                    if (oldUserAccount == null) continue;
                    Console.WriteLine($"Adding: {oldUserAccount.Username}");
                    newUserAccounts.Add(new User
                    {
                        Id = oldUserAccount.Id,
                        CommandUsed = DateTime.Now.AddDays(-1),
                        Name = oldUserAccount.Username,
                        SpamWarning = 0,
                        TotalTimesTimedOut = 0
                    });
                    userInfos.Add(new UserInfo
                    {
                        UserId = oldUserAccount.Id,
                        ServerId = guild.Id,
                        LastMessageSend = DateTime.Now,
                        LastVoiceStateUpdateReceived = DateTime.Now,
                        Level = oldUserAccount.Level,
                        MessagesSend = 0,
                        TimeConnected = oldUserAccount.TotalTimeConnected,
                        Xp = oldUserAccount.Xp
                    });
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Adding: {oldUserAccount.Username}");
                    Console.ResetColor();
                }

                Console.WriteLine($"Saving {newUserAccounts.Count} newUserAccounts and {userInfos.Count} userInfos");
                await unitOfWork.Users.AddRangeAsync(newUserAccounts).ConfigureAwait(false);
                await unitOfWork.UserInfos.AddRangeAsync(userInfos).ConfigureAwait(false);

                await unitOfWork.SaveAsync().ConfigureAwait(false);
                Console.WriteLine("Data saved!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
