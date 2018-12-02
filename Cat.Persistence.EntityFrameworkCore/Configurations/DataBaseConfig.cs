using System.IO;
using Newtonsoft.Json;

namespace Cat.Persistence.EntityFrameworkCore.Configurations
{
    public class DatabaseConfig
    {
        private const string ConfigFolder = "Configs";
        private const string ConfigFile = "DatabaseConfig.json";

        public static Config Data;

        static DatabaseConfig()
        {
            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);

            if (!File.Exists(ConfigFolder + "/" + ConfigFile))
            {
                Data = new Config();
                var json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
            }
            else
            {
                var json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
                Data = JsonConvert.DeserializeObject<Config>(json);
            }
        }
    }

    public struct Config
    {
        public string ConnectionString;
    }
}