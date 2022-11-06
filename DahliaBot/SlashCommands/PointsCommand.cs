using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DahliaBot
{
    public class PointsCommand : DahliaSlashCommand
    {
        public override string Name => "points";

        public override SlashCommandBuilder Builder(IServiceProvider provider)
        {
            var pointsCommand = new SlashCommandBuilder();
            pointsCommand.WithName(Name).WithDescription("Display user points").AddOption(
                new SlashCommandOptionBuilder()
                .WithName("user")
                .WithDescription("User to display points of")
                .WithRequired(true)
                .WithType(ApplicationCommandOptionType.User));
            return pointsCommand;
        }
        public async override Task Run(SocketSlashCommand command, IServiceProvider provider)
        {
            var user = command.Data.Options.First().Value as SocketGuildUser;
            var context = provider.GetService<DahliaContext>();
            var serverUser = await context.GetUser(user.Id);
            var sb = new StringBuilder();
            if (serverUser.PointCategories != null)
            {
                foreach (var userPointCategory in serverUser.PointCategories)
                    sb.Append($"{userPointCategory.Category.Name} - {userPointCategory.Points}\n");
            }
            
            var embedBuilder = new EmbedBuilder()
               .WithTitle($"{user.Username}'s Points:")
               .WithDescription(sb.ToString())
               .WithColor(Color.DarkOrange)
               .WithCurrentTimestamp();

            await command.RespondAsync(embed: embedBuilder.Build());
        }
    }
}