using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DahliaBot
{
    public interface IDahliaSlashCommand
    {
        public abstract string Name { get; }
        public abstract SlashCommandBuilder Builder(IServiceProvider provider);
        public abstract Task Run(SocketSlashCommand command, IServiceProvider provider);
    }
}
