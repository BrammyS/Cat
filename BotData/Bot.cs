using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cat.ServerAccounts;
using Discord.WebSocket;

namespace Cat.BotData
{
    public class Bot
    {
        private static List<BotData> _bot;

        private static string accountsFile = "Data/BotData/MiscData.json";

        public static void SaveAccounts()
        {
            Datastorage.SaveBotData(_bot, accountsFile);
        }

        public static BotData GetAccount()
        {
            if (Datastorage.SaveExists(accountsFile))
            {
                _bot = Datastorage.LoadBotData(accountsFile).ToList();
            }
            else
            {
                _bot = new List<BotData>();
                SaveAccounts();
            }
            return GetOrCreateAccount();
        }

        private static BotData GetOrCreateAccount()
        {
            var result = from a in _bot
                         where a.ID == 1
                         select a;

            var account = result.FirstOrDefault();
            if (account == null) account = CreateServerAccount();
            return account;
        }

        private static BotData CreateServerAccount()
        {
            var newAccount = new BotData()
            {
                ID = 1,
                Guilds = new ulong[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };

            _bot.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}
