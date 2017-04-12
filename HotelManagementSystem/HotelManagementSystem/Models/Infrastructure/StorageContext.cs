﻿using HotelManagementSystem.Models.Entities.Identity;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HotelManagementSystem.Models.Infrastructure
{
    public class StorageContext : DbContext
    {

        public DbSet<Rule> Rules { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Case> Cases { get; set; }
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
