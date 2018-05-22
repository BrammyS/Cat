using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Cat.UserAccounts
{
    public static class UserAccounts
    {
        public static List<UserAccount> accounts;

        public static void SaveAccounts(SocketUser user, SocketGuild guild)
        {
            string accountsFile = $"Data/UserData/{guild.Id}/{user.Id}.json";
            Datastorage.SaveUserAccounts(accounts, accountsFile);
        }

        public static UserAccount GetAccount(SocketUser user, SocketGuild guild)
        {
            string accountsFile = $"Data/UserData/{guild.Id}/{user.Id}.json";
            if (Datastorage.SaveExists(accountsFile))
            {
                accounts = Datastorage.LoadUserAccounts(accountsFile).ToList();
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory($"Data/UserData/{guild.Id}");
                accounts = new List<UserAccount>();
                SaveAccounts(user, guild);
            }
            return GetOrCreateAccount(user.Id, user, guild);
        }

        private static UserAccount GetOrCreateAccount(ulong id, SocketUser user, SocketGuild guild)
        {
            var result = from a in accounts
                where a.ID == id
                select a;

            var account = result.FirstOrDefault();
            if (account == null) account = CreateUserAccount(id, user, guild);
            return account;
        }

        private static UserAccount CreateUserAccount(ulong id, SocketUser user, SocketGuild guild)
        {
            var newAccount = new UserAccount()
            {
                ID = id,
                Username = user.Username,
                Level = 1,
                XP = 1,
                TimeConnected = DateTime.Now,
                TotalTimeConnected = 0,
                LastEmote = DateTime.Now,
                TimeConnectedWeek = 0,
                TimeConnectedMonth = 0,
                TimeConnectedYear = 0,
                LastCommandUsed = DateTime.Now
            };

            accounts.Add(newAccount);
            SaveAccounts(user, guild);
            return newAccount;
        }
    }
}
