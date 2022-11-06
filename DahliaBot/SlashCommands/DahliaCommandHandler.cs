using DahliaBot.Servives;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DahliaBot
{
    public class DahliaCommandHandler
    {
        private readonly SocketSlashCommand _command;
        private readonly IServiceProvider _provider;
        public DahliaCommandHandler(SocketSlashCommand command, IServiceProvider provider)
        {
            _command = command;
            _provider = provider;
        }

        public async Task Handle()
        {
            var command = DahliaSlashCommand.All.First(c => c.Name == _command.CommandName);
            await command.Run(_command, _provider);
            
            var context = _provider.GetService<DahliaContext>();
            await _provider.GetService<UserPointService>().GrantPoints(await context.GetUser(_command.User.Id),context.PointCategories.First(c=>c.Name=="Dahlia"),10);
        }
    }
}