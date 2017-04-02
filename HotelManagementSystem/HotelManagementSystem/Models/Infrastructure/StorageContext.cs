using HotelManagementSystem.Models.Entities.Identity;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Models.Infrastructure
{
    public class StorageContext : DbContext
    {

        public DbSet<Rule> Rules { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Task> Tasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=hotelmanagementsystem.database.windows.net;" +
                "Initial Catalog=HotelManagementSystem;" +
                "Persist Security Info=True;" +
                "User ID=hmsuser;" +
                "Password=Al315t3r<r0wl3y");
        }
    }

    public static class DbInitializer
    {
        public static void Initialize(StorageContext context)
        {
            if (context.Rules.Any())
            {
                return; //Db has been seeded
            }

            var rules = new Rule[]
            {
                new Rule{RuleID=Guid.NewGuid(),Name="Restaurant",Description="Restaurant is open at hours: 8 AM-10 PM"},
                new Rule{RuleID=Guid.NewGuid(),Name="Evacuation",Description="In case of seeing fire please run to the hills" },
                new Rule{RuleID=Guid.NewGuid(),Name="Technical Issues",Description="If something does not work, please don't touch it. Same if it's on fire. Better call Mr. Mieciu"},
                new Rule{RuleID=Guid.NewGuid(),Name="Room Cleaning",Description="Rooms are cleaned always on odd days of month"}
            };
            foreach(var rule in rules)
            {
                context.Rules.Add(rule);
            }
            context.SaveChanges();
        }
    }

}
