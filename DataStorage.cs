using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cat.BotData;
using Cat.ServerAccounts;
using Cat.UserAccounts;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Cat
{
    public class Datastorage
    {
        private static object _lock = new object();
        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
        /*************************************UserDataStorage*********************************/
        public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath)
        {
            lock (_lock)
            {
                string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        public static IEnumerable<UserAccount> LoadUserAccounts(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserAccount>>(json);
        }
        /*************************************ServerDataStorage*********************************/
        public static void SaveServerAccounts(IEnumerable<ServerAccount> accounts, string filePath)
        {
            lock (_lock)
            {
                string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        public static IEnumerable<ServerAccount> LoadServerAccounts(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<ServerAccount>>(json);
        }
        /*************************************BotDataStorage*********************************/
        public static void SaveBotData(IEnumerable<BotData.BotData> accounts, string filePath)
        {
            lock (_lock)
            {
                string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        public static IEnumerable<BotData.BotData> LoadBotData(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<BotData.BotData>>(json);
        }
    }
}