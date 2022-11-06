using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DahliaBot.Entities.UserPointCategory;

namespace DahliaBot.Entities
{
    public class PointCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public PointCategoryType Type { get; set; }
        public string ChannelIdsCSV { get; set; }



        public static List<PointCategory> DefaultCategories = new List<PointCategory>() {
            new PointCategory() {Name="General", Type=PointCategoryType.special},
            new PointCategory() {Name="Gaming", Type=PointCategoryType.channels, ChannelIdsCSV="" },
            new PointCategory() {Name="Dahlia", Type=PointCategoryType.channels, ChannelIdsCSV="" },
            new PointCategory() {Name="Wholesome", Type=PointCategoryType.channels, ChannelIdsCSV="" },
            new PointCategory() {Name="Creative", Type=PointCategoryType.channels, ChannelIdsCSV="" },
        };

        public static void SetUpDefaultCategories(ServerUser user)
        {
            user.PointCategories = new List<UserPointCategory>();
            foreach (var category in DefaultCategories) user.PointCategories.Add(new UserPointCategory() { Category = category });    
        }
    }
}