using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Cat.Discord.Services
{
    public interface IDiscordLogger
    {
        void Log(string folder, IResult result, ICommandContext context);
        Task Log(LogMessage logMsg);
    }
}