using System.Threading.Tasks;
using Cat.Discord.Interfaces;
using Cat.Interfaces;

namespace Cat
{
    public class Cat : ICat
    {
        private readonly IConnection _connection;

        public Cat(IConnection connection)
        {
            _connection = connection;
        }

        public async Task StartAsync()
        {
            await _connection.ConnectAsync().ConfigureAwait(false);
        }
    }
}
