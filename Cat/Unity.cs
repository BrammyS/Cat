using Cat.Discord;
using Cat.Discord.Configurations;
using Cat.Discord.Handlers;
using Cat.Discord.Interfaces;
using Cat.Discord.Services;
using Cat.Discord.Services.Implementations;
using Cat.Interfaces;
using Cat.Services;
using Cat.Services.Implementations;
using Discord.WebSocket;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Resolution;

namespace Cat
{
    public static class Unity
    {
        private static UnityContainer container;

        private static UnityContainer Container
        {
            get
            {
                if (container == null)
                    RegisterTypes();
                return container;
            }
        }
        public static void RegisterTypes()
        {
            container = new UnityContainer();
            container.RegisterType<ILogger, Logger>(new PerThreadLifetimeManager());

            container.RegisterSingleton<DiscordSocketConfig>(new InjectionFactory(i => SocketConfig.GetDefault()));
            container.RegisterSingleton<DiscordShardedClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));

            container.RegisterSingleton<IConnection, Connection>();
            container.RegisterSingleton<ICommandHandler, CommandHandler>();
            container.RegisterSingleton<ICat, Cat>();

            container.RegisterType<IDiscordLogger, DiscordLogger>(new PerThreadLifetimeManager());
        }

        public static T Resolve<T>()
        {
            return (T)Container.Resolve(typeof(T), string.Empty, new CompositeResolverOverride());
        }
    }
}
