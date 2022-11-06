using DahliaBot.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DahliaBot
{
    public class DahliaDatabaseConfigurator
    {
        private readonly IServiceProvider _provider;
        private readonly DahliaContext _context;
        public DahliaDatabaseConfigurator(IServiceProvider provider)
        {
            _provider = provider;
            _context = _provider.GetService<DahliaContext>();
        }
        public async Task Create() {
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task Maintanance() {
            await _context.Database.EnsureCreatedAsync();

            foreach (var category in PointCategory.DefaultCategories)
            {
                if (!_context.PointCategories.Any(c => c.Name == category.Name)) await _context.PointCategories.AddAsync(category);
            }
            await _context.SaveChangesAsync();
        }
    }
}