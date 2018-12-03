using System.Collections.Generic;
using System.IO;
using Cat.Persistence.JsonStorage.UserAccounts;
using Newtonsoft.Json;

namespace Cat.Persistence.JsonStorage
{
    public class Datastorage
    {
        private static readonly object Lock = new object();
        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
        /*************************************UserDataStorage*********************************/
        public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath)
        {
            lock (Lock)
            {
                var json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        public static IEnumerable<UserAccount> LoadUserAccounts(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserAccount>>(json);
        }
    }
}