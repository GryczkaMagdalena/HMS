using FluentScheduler;
using HotelManagementSystem.Models.Abstract;
using HotelManagementSystem.Models.Infrastructure.Scheduler.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Infrastructure.Scheduler
{
    public class JobScheduler : Registry
    {
        public JobScheduler(IdentityContext context,IUserService userService)
        {
            Schedule(() =>
            {
                HandleNotFinishedTasks job = new HandleNotFinishedTasks(context, userService);
                job.Execute();
            }).ToRunNow().AndEvery(5).Minutes();
        }
    }
}
