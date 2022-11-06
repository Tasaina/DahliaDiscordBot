using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DahliaBot.Entities
{
    public class ServerUser
    {
        public ServerUser() { }
        public ServerUser(ulong id)
        {
            Id = id;
        }

        [Key]
        public ulong Id { get; set; }
        public DateTime LastActivity { get; set; }
        public virtual List<UserPointCategory> PointCategories { get; set; }
        public DateTime LastDailyUsed { get; set; }
    }
}