using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord.WebSocket;

namespace Cat.Persistence.JsonStorage.UserAccounts
{
    public static class UserAccounts
    {
        public static List<UserAccount> Accounts;

        public static void SaveAccounts(SocketUser user, SocketGuild guild)
        {
            var accountsFile = $"Data/UserData/{guild.Id}/{user.Id}.json";
            Datastorage.SaveUserAccounts(Accounts, accountsFile);
        }

        public static UserAccount GetAccount(SocketUser user, SocketGuild guild)
        {
            var accountsFile = $"Data/UserData/{guild.Id}/{user.Id}.json";
            if (Datastorage.SaveExists(accountsFile))
            {
                Accounts = Datastorage.LoadUserAccounts(accountsFile).ToList();
            }
            else
            {
                Directory.CreateDirectory($"Data/UserData/{guild.Id}");
                Accounts = new List<UserAccount>();
                SaveAccounts(user, guild);
            }
            return GetOrCreateAccount(user.Id, user, guild);
        }

        private static UserAccount GetOrCreateAccount(ulong id, SocketUser user, SocketGuild guild)
        {
            var result = from a in Accounts
                         where a.Id == id
                         select a;

            var account = result.FirstOrDefault();
            return account;
        }

        private static UserAccount CreateUserAccount(ulong id, SocketUser user, SocketGuild guild)
        {
            var newAccount = new UserAccount()
            {
                Id = id,
                Username = user.Username,
                Level = 1,
                Xp = 1,
                TimeConnected = DateTime.Now,
                TotalTimeConnected = 0,
                LastEmote = DateTime.Now,
                TimeConnectedWeek = 0,
                TimeConnectedMonth = 0,
                TimeConnectedYear = 0,
                LastCommandUsed = DateTime.Now
            };

            Accounts.Add(newAccount);
            SaveAccounts(user, guild);
            return newAccount;
        }
    }
}