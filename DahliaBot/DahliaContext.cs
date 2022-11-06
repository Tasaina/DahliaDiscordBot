using DahliaBot.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DahliaBot
{
    public class DahliaContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<ServerUser> Users { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<PointCategory> PointCategories { get; set; }
        public DbSet<UserPointCategory> UserPointCategories { get; set; }

        public DahliaContext() : base() { }

        public DahliaContext(DbContextOptions<DahliaContext> options): base(options){ }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServerUser>().HasMany(su=>su.PointCategories);
            modelBuilder.Entity<UserPointCategory>().HasOne(upc => upc.Category);
        }

        public async Task InstantiateUser(ulong id)
        {
            var newUser = new ServerUser(id);
            newUser.PointCategories = new List<UserPointCategory>();
            foreach (var category in PointCategories)
            {
                newUser.PointCategories.Add(new UserPointCategory() { Category=category });
            }
            await Users.AddAsync(newUser);
        }

        public async Task<ServerUser> GetUser(ulong id)
        {
            var user = await Users.FindAsync(id);
            if (user == null) await InstantiateUser(id);
            return await Users.FindAsync(id);
        }

        public async Task InstantiatePlayer(ulong id)
        {
            var newPlayer = new Player(id);
            await Players.AddAsync(newPlayer);
        }

        public async Task<Server> GetServerFromGuildId(ulong guildId)
        {
            return await Servers.FirstOrDefaultAsync(s => s.Id == guildId);
        }
    }
}