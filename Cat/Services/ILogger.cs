namespace Cat.Services
{
    public interface ILogger
    {
        void Log(string message);
        void Log(string folder, string text);
    }
}