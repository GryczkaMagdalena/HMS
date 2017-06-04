using HotelManagementSystem.Models.Entities.Identity;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Infrastructure
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext()
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.EnsureDeleted();
                Database.Migrate();
            }
        }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public new DbSet<User> Users { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Entities.Storage.Task> Tasks { get; set; }
        public DbSet<Case> Cases { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=hotelmanagementsystemdeploy.database.windows.net;" +
                "Initial Catalog=HotelManagementSystemDeploy;" +
                "Persist Security Info=True;" +
                "User ID=hmsuser;" +
                "Password=Al315t3r<r0wl3y");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<string>>().HasKey(q => new { q.UserId });
            builder.Entity<Manager>().HasMany(q => q.Shifts);
            builder.Entity<Worker>().HasMany(q => q.Shifts);
            base.OnModelCreating(builder);
        }
    }

}
