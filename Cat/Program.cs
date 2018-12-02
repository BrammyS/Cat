using System.Threading.Tasks;
using Cat.Interfaces;

namespace Cat
{
    public class Program
    {
        static async Task Main()
        {

            Unity.RegisterTypes();
            var bot = Unity.Resolve<ICat>();
            await bot.StartAsync().ConfigureAwait(false);
        }
    }
}
