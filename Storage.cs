using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Cat
{
    public class Storage
    {
        /*************************************CustomData*********************************/
        private static Dictionary<string, string> pairs = new Dictionary<string, string>();

        public static string filePath = $"Data/BotData/MiscInfo.json";

        static Storage()
        {
            if (!ValidateStorageFile(filePath)) return;
            string json = File.ReadAllText(filePath);
            pairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static void AddPairToStorage(string key, string value)
        {
            pairs.Add(key, value);
            SaveData();
        }

        public static void SaveData()
        {
            string json = JsonConvert.SerializeObject(pairs, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private static bool ValidateStorageFile(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveData();
                return false;
            }
            return true;
        }
        public static string GetFormattedAlert(string key, params object[] parameter)
        {
            if (pairs.ContainsKey(key))
            {
                return String.Format(pairs[key], parameter);
            }
            return "";
        }
    }
}
