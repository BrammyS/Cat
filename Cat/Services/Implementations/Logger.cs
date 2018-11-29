using System;
using System.IO;
using System.Threading.Tasks;

namespace Cat.Services.Implementations
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Task.Run(() =>
            {
                Console.WriteLine($"{DateTime.Now:hh:mm:ss.fff} : " + message);
                Log("Misc", message);
            });
        }

        public void Log(string folder, string text)
        {
            Task.Run(() =>
            {
                var filePath = $"Logs/{folder}/{DateTime.Now:MMMM, yyyy}";
                if (!File.Exists(filePath)) Directory.CreateDirectory(filePath);
                filePath += $"/{DateTime.Now:dddd, MMMM d, yyyy}.txt";
                using (var file = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    using (var sw = new StreamWriter(file))
                    {
                        sw.WriteLine($"{DateTime.Now:T} : {text}");
                    }
                    file.Flush();
                    file.Close();
                }
            });
        }
    }
}
