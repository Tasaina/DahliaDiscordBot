using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DahliaBot
{
    public class CategoriesCommand : DahliaSlashCommand
    {
        public override string Name => "categories";

        public override SlashCommandBuilder Builder(IServiceProvider provider)
        {
            var pointsCommand = new SlashCommandBuilder();
            pointsCommand.WithName(Name).WithDescription("Display point categories");
            return pointsCommand;
        }
        public async override Task Run(SocketSlashCommand command, IServiceProvider provider)
        {
            var context = provider.GetService<DahliaContext>();
            var categories = await context.PointCategories.ToListAsync();
            var sb = new StringBuilder();
            foreach (var category in categories) sb.Append($"{category.Name} \n");
            
            var embedBuilder = new EmbedBuilder()
               .WithTitle("Categories")
               .WithDescription(sb.ToString())
               .WithColor(Color.DarkOrange)
               .WithCurrentTimestamp();

            await command.RespondAsync(embed: embedBuilder.Build());
        }
    }
}