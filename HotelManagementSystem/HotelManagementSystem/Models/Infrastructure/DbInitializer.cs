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
    public static class DbInitializer
    {
        //public static void Initialize(IdentityContext context)
        //{
        //    if (context.Rules.Any())
        //    {
        //        return; //Db has been seeded
        //    }

        //    var rules = new Rule[]
        //    {
        //        new Rule{RuleID=Guid.NewGuid(),Name="Restaurant",Description="Restaurant is open at hours: 8 AM-10 PM"},
        //        new Rule{RuleID=Guid.NewGuid(),Name="Evacuation",Description="In case of seeing fire please run to the hills" },
        //        new Rule{RuleID=Guid.NewGuid(),Name="Technical Issues",Description="If something does not work, please don't touch it. Same if it's on fire. Better call Mr. Mieciu"},
        //        new Rule{RuleID=Guid.NewGuid(),Name="Room Cleaning",Description="Rooms are cleaned always on odd days of month"}
        //    };
        //    foreach (var rule in rules)
        //    {
        //        context.Rules.Add(rule);
        //    }
        //    context.SaveChanges();
        //}

        public static void Initialize(IdentityContext context)
        {
            if (context.Roles.Any())
            {
                return;
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
            var roles = new IdentityRole[]
            {
                new IdentityRole{Id=Guid.NewGuid().ToString(),Name="admin",NormalizedName="Administrator"},
                new IdentityRole{Id=Guid.NewGuid().ToString(),Name="manager",NormalizedName="Manager"},
                new IdentityRole{Id=Guid.NewGuid().ToString(),Name="worker",NormalizedName="Worker"},
                new IdentityRole{Id=Guid.NewGuid().ToString(),Name="customer",NormalizedName="Customer"}
            };
            foreach (var role in roles)
            {
                context.Roles.Add(role);
            }
            context.SaveChanges();
        }
        private static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
        public static async Task<bool> AddWorkerShifts(IdentityContext context)
        {
            List<User> workers = await context.Users
                .Where(q => q.WorkerType == WorkerType.Cleaner || q.WorkerType == WorkerType.Technician)
                .Include(q=>q.Shifts).ToListAsync();

            if (!workers.Any(q=>q.Shifts!=null &&q.Shifts.Any(p=>p.StartTime>DateTime.Now || p.EndTime<DateTime.Now)))
            {
                var now = DateTime.Now;
                for (int i = 0; i < workers.Count; i++)
                {
                    User worker = workers[i];
                    int switcher = i % 3;
                    switch (switcher)
                    {
                        case 0:
                            for (int j = 0; j < 7; j++)
                            {
                                var startTime = now.AddDays(j);
                                if (worker.Shifts == null) worker.Shifts = new List<Shift>();
                                worker.Shifts.Add(new Shift()
                                {
                                    Break = false,
                                    StartTime = RoundUp(startTime, TimeSpan.FromMinutes(30)),
                                    EndTime = RoundUp(startTime, TimeSpan.FromMinutes(30)).AddHours(8),
                                    ShiftID = Guid.NewGuid(),
                                    UserID=worker.Id
                                });
                            }
                            break;
                        case 1:
                            for (int j = 0; j < 7; j++)
                            {
                                var startTime = now.AddDays(j).AddHours(8);
                                if (worker.Shifts == null) worker.Shifts = new List<Shift>();
                                worker.Shifts.Add(new Shift()
                                {
                                    Break = false,
                                    StartTime = RoundUp(startTime, TimeSpan.FromMinutes(30)),
                                    EndTime = RoundUp(startTime, TimeSpan.FromMinutes(30)).AddHours(8),
                                    ShiftID = Guid.NewGuid(),
                                    UserID = worker.Id
                                });
                            }
                            break;
                        case 2:
                            for (int j = 0; j < 7; j++)
                            {
                                var startTime = now.AddDays(j).AddHours(16);
                                if (worker.Shifts == null) worker.Shifts = new List<Shift>();
                                worker.Shifts.Add(new Shift()
                                {
                                    Break = false,
                                    StartTime = RoundUp(startTime, TimeSpan.FromMinutes(30)),
                                    EndTime = RoundUp(startTime, TimeSpan.FromMinutes(30)).AddHours(8),
                                    ShiftID = Guid.NewGuid(),
                                    UserID = worker.Id
                                });
                            }
                            break;
                    }
                }
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
