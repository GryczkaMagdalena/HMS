using HotelManagementSystem.Models.Entities.Identity;
using HotelManagementSystem.Models.Entities.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Infrastructure
{
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
            foreach (var rule in rules)
            {
                context.Rules.Add(rule);
            }
            context.SaveChanges();
        }

        public static void Initialize(IdentityContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            var roles = new Role[]
            {
                new Role{Id=Guid.NewGuid().ToString(),Name="admin",NormalizedName="Administrator"},
                new Role{Id=Guid.NewGuid().ToString(),Name="manager",NormalizedName="Manager"},
                new Role{Id=Guid.NewGuid().ToString(),Name="worker",NormalizedName="Worker"},
                new Role{Id=Guid.NewGuid().ToString(),Name="customer",NormalizedName="Customer"}
            };
            foreach(var role in roles)
            {
                context.Roles.Add(role);
            }
            context.SaveChanges();
        }
    }
}
