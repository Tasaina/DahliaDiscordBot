using System;
using System.Collections.Generic;
using System.Text;

namespace DahliaBot.Entities
{
    public static class PointCategoryExtensions
    {
        public static List<ulong> IDsFromCSV(this string csv)
        {
            if (csv == null) return new List<ulong>();
            var channelIds = new List<ulong>();
            var splitIds = csv.Replace(" ", "").Split(',');
            foreach (var idString in splitIds)
            {
                channelIds.Add(ulong.Parse(idString));
            }
            return channelIds;
        }
    }
}
