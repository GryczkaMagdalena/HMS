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
            }).ToRunNow().AndEvery(5).Minutes();

            Schedule(() =>
            {
                UpdatePriority job = new UpdatePriority(context);
                job.Execute();
            }).ToRunNow().AndEvery(10).Minutes();

            Schedule(() =>
            {
                CollectFinished job = new CollectFinished(context);
                job.Execute();
            }).ToRunNow().AndEvery(15).Minutes();

            Schedule(() =>
            {
                MoveOldAssigned job = new MoveOldAssigned(context, userService);
                job.Execute();
            }).ToRunNow().AndEvery(8).Minutes();

            Schedule(() =>
            {
                AssignUnassigned job = new AssignUnassigned(context, userService);
                job.Execute();
            }).ToRunNow().AndEvery(30).Minutes();
        }
    }
}
