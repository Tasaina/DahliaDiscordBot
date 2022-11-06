using DahliaBot.Servives;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DahliaBot
{
    public class DailyCommand : DahliaSlashCommand
    {
        private const int dailyAmount = 50;

        public override string Name => "daily";

        public override SlashCommandBuilder Builder(IServiceProvider provider)
        {
            var pointsCommand = new SlashCommandBuilder();
            pointsCommand.WithName(Name).WithDescription("Grant a user points");
            pointsCommand.AddOption(
                new SlashCommandOptionBuilder()
                .WithName("user")
                .WithDescription("User to give points to")
                .WithRequired(true)
                .WithType(ApplicationCommandOptionType.User));

            var categoryOptions = new SlashCommandOptionBuilder()
                .WithName("category")
                .WithDescription("Category to add points to")
                .WithRequired(true)
                .WithType(ApplicationCommandOptionType.String);

            var context = provider.GetService<DahliaContext>();
            foreach (var category in context.PointCategories.ToList())
            {
                categoryOptions.AddChoice(category.Name, category.Name);
            }

            pointsCommand.AddOption(categoryOptions);
            return pointsCommand;
        }
        public async override Task Run(SocketSlashCommand command, IServiceProvider provider)
        {
            var data = command.Data.Options.ToList();
            var context = provider.GetService<DahliaContext>();
            var requestingUser = await context.GetUser(command.User.Id);
            
            if (requestingUser.LastDailyUsed.Date==DateTime.Now.Date)
            {
                await command.RespondAsync("You already used your daily");
                return;
            }
            var categoryName = data[1].Value as string;
            categoryName = char.ToUpper(categoryName[0]) + categoryName.Substring(1);
            var category = await context.PointCategories.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (category == null)
            {
                await command.RespondAsync("Category not found");
                return;
            }
            var targetUser = data[0].Value as SocketGuildUser;
            var targetServerUser = await context.GetUser(targetUser.Id);
            var pointService = provider.GetService<UserPointService>();
            await pointService.GrantPoints(targetServerUser, category, dailyAmount);
            requestingUser.LastDailyUsed = DateTime.Now;
            await command.RespondAsync($"{targetUser.Nickname} has been granted {dailyAmount} {category.Name} points!");
        }
    }
}