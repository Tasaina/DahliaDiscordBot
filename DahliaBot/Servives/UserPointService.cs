using DahliaBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DahliaBot.Servives
{
    public class UserPointService
    {
        private readonly DahliaContext _context;

        public UserPointService(DahliaContext context) {
            _context = context;
        }

        public async Task GrantPoints(ServerUser user, PointCategory category, int points) {
            if (user.PointCategories == null) user.PointCategories = new List<UserPointCategory>();
            var userPointCategory = user.PointCategories.FirstOrDefault(pc => pc.Category == category);
            if (userPointCategory == null)
            {
                userPointCategory = new UserPointCategory { Category = category };
                user.PointCategories.Add(userPointCategory);
            }
            userPointCategory.Points+=points;
            await _context.SaveChangesAsync();
        }
    }
}
