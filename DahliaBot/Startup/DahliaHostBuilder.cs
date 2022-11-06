using DahliaBot.Servives;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DahliaBot
{
    public static class DahliaHostBuilder
    {
        public static IHost Build(IConfigurationRoot config)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                services
                .AddSingleton(config)
                .AddSingleton(dsc => new DiscordSocketClient(new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.AllUnprivileged,
                    AlwaysDownloadUsers = true,
                }))
                .AddDbContext<DahliaContext>(options => options.UseSqlServer(config["database:connectionString"]).UseLazyLoadingProxies())
                .AddScoped<UserPointService>()
                ).Build();
        }
    }
}