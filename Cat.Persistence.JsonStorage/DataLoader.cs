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
                var users = new List<User>();
                var guild = client.GetGuild(224949218639478784);
                Console.WriteLine($"guild: {guild.Name}");
                var oldUserAccounts = new List<UserAccount>();
                oldUserAccounts.AddRange(guild.Users.Select(user =>
                {
                    Console.WriteLine($"Loading: {user.Id}");
                    return UserAccounts.UserAccounts.GetAccount(user, guild);
                }));
                foreach (var oldUserAccount in oldUserAccounts)
                {
                    if (oldUserAccount?.Username == null) continue;
                    Console.WriteLine($"Adding: {oldUserAccount.Id}");
                    users.Add(new User
                    {
                        UserId = oldUserAccount.Id,
                        ServerId = guild.Id,
                        LastMessageSend = DateTime.Now,
                        LastVoiceStateUpdateReceived = DateTime.Now,
                        Level = oldUserAccount.Level,
                        MessagesSend = 0,
                        TimeConnected = oldUserAccount.TotalTimeConnected,
                        Xp = oldUserAccount.Xp,
                        CommandUsed = DateTime.Now.AddDays(-1),
                        Name = oldUserAccount.Username,
                        SpamWarning = 0
                    });
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Adding: {oldUserAccount.Username}");
                    Console.ResetColor();
                }

                Console.WriteLine($"Saving {users.Count} newUserAccounts");
                await unitOfWork.Users.AddRangeAsync(users).ConfigureAwait(false);

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
