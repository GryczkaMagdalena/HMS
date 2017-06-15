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
        public JobScheduler(IdentityContext context, IUserService userService)
        {
            Schedule(() =>
            {
                HandleNotFinishedTasks job = new HandleNotFinishedTasks(context, userService);
                job.Execute();
            }).ToRunEvery(5).Minutes();

            Schedule(() =>
            {
                UpdatePriority job = new UpdatePriority(context);
                job.Execute();
            }).ToRunEvery(10).Minutes();

            Schedule(() =>
            {
                CollectFinished job = new CollectFinished(context);
                job.Execute();
            }).ToRunEvery(15).Minutes();

            Schedule(() =>
            {
                MoveOldAssigned job = new MoveOldAssigned(context, userService);
                job.Execute();
            }).ToRunEvery(8).Minutes();

            Schedule(() =>
            {
                AssignUnassigned job = new AssignUnassigned(context, userService);
                job.Execute();
            }).ToRunEvery(30).Minutes();
        }
    }
}
