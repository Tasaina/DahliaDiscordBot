using DahliaBot.Entities;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DahliaBot
{
    class Program
    {
        private DiscordSocketClient _client;
        private IServiceProvider _provider;
        
        public static void Main()
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json")
                .Build();

            using IHost host = DahliaHostBuilder.Build(config);
            using IServiceScope serviceScope = host.Services.CreateScope();
            _provider = serviceScope.ServiceProvider;
            
            DahliaSlashCommand.PreloadCommands();

            var databaseConfigurator = new DahliaDatabaseConfigurator(_provider);
            await databaseConfigurator.Create();
            await databaseConfigurator.Maintanance();

            await RunBotAsync();
        }

        public async Task RunBotAsync()
        {
            _client = _provider.GetRequiredService<DiscordSocketClient>();
            _client.Log += Log;

            var config = _provider.GetRequiredService<IConfigurationRoot>();

            await _client.LoginAsync(TokenType.Bot, config["tokens:dahliaBot"]);
            await _client.StartAsync();

            _client.Ready += ClientReady;
            _client.SlashCommandExecuted += SlashCommand;
            _client.MessageReceived += MessageReceived;

            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            await _client.SetGameAsync("dumb");

            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.IsBot) return;
            var processor = new DahliaMessageProcessor(message, _provider);
            await processor.ProcessMessage();
        }

        private async Task SlashCommand(SocketSlashCommand command)
        {
            var handler = new DahliaCommandHandler(command, _provider);
            await handler.Handle();
        }

        private async Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
        }

        private async Task ClientReady()
        {
            Console.WriteLine("Client ready, starting setup");

            var commands = DahliaSlashCommand.All.Select(c=>c.Builder(_provider));
            foreach (var command in commands)
            {
                try
                {
                    await _client.CreateGlobalApplicationCommandAsync(command.Build());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            var outdatedCommands = (await _client.GetGlobalApplicationCommandsAsync())
                .Where(oc => !commands.Any(c => c.Name == oc.Name))
                .ToList();
            outdatedCommands.ForEach(async c => await c.DeleteAsync());

            Console.WriteLine("Client setup complete");
        }
    }
}