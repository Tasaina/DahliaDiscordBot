using DahliaBot.Entities;
using DahliaBot.Servives;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DahliaBot.Entities.UserPointCategory;

namespace DahliaBot
{
    public class DahliaMessageProcessor
    {
        private IServiceProvider _provider;
        private SocketMessage _message;
        private DahliaContext _context;
        private UserPointService _pointsService;
        private bool _pointsGranted;

        public DahliaMessageProcessor(SocketMessage message, IServiceProvider provider)
        {
            _provider = provider;
            _message = message;
            _context = _provider.GetService<DahliaContext>();
            _pointsService = _provider.GetService<UserPointService>();
        }

        public async Task ProcessMessage()
        {
            var user = await _context.Users.FindAsync(_message.Author.Id);
            if (user == null) await _context.InstantiateUser(_message.Author.Id);
            user = await _context.Users.FindAsync(_message.Author.Id);
            await GrantCategoryPoints(user);
            await _context.SaveChangesAsync();
        }

        private async Task GrantCategoryPoints(ServerUser user)
        {
            if (user.PointCategories == null) user.PointCategories=new List<UserPointCategory>();

            var matchingCategories = await _context.PointCategories
                                        .AsAsyncEnumerable()
                                        .Where(c => 
                                            c.ChannelIdsCSV.IDsFromCSV()
                                            .Contains(_message.Channel.Id))
                                        .ToListAsync();
            foreach (var category in matchingCategories) await GrantCategoryPoints(user, category);

            var generalCategory = _context.PointCategories.FirstOrDefault(pc => pc.Name == "General");
            if (!_pointsGranted) await GrantCategoryPoints(user, generalCategory);
        }

        private async Task GrantCategoryPoints(ServerUser user, PointCategory category)
        {
            await _pointsService.GrantPoints(user,category,1);
            _pointsGranted = true;
        }

    }
}