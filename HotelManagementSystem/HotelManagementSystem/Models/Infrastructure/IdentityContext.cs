﻿using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Infrastructure
{
    public class IdentityContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
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
