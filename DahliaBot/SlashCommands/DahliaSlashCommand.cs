using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DahliaBot
{
    public abstract class DahliaSlashCommand : IDahliaSlashCommand, IComparable<DahliaSlashCommand>
    {
        public abstract string Name { get; }
        public static IEnumerable<DahliaSlashCommand> All = null;
        public static void PreloadCommands()
        {
            if (All == null) All = ReflectiveEnumerator.GetEnumerableOfType<DahliaSlashCommand>();
        }

        public abstract SlashCommandBuilder Builder(IServiceProvider provider);

        public abstract Task Run(SocketSlashCommand command, IServiceProvider provider);

        public int CompareTo([AllowNull] DahliaSlashCommand other)
        {
            if (other == null) return 1;
            return 0;
        }
    }
}

