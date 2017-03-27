using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Models.Infrastructure
{
    public class StorageContext : DbContext
    {
        public new DbSet<Room> Rooms { get; set; }
        public new DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=hotelmanagementsystem.database.windows.net;" +
                "Initial Catalog=HotelManagementSystem;" +
                "Persist Security Info=True;" +
                "User ID=hmsuser;" +
                "Password=Al315t3r<r0wl3y");
        }
    }
}
