using FluentScheduler;
using HotelManagementSystem.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Helpers;
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Models.Infrastructure.Scheduler.Jobs
{
    public class HandleNotFinishedTasks : IJob
    {
        private readonly IdentityContext _context;
        private readonly IUserService _userService;
        private readonly TaskDisposer _taskDisposer;
        public HandleNotFinishedTasks(IdentityContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
            _taskDisposer = new TaskDisposer(userService, context);
        }

        public  void Execute()
        {
            foreach (var worker in _context.LazyLoadWorkers().Result)
            {
                if (worker.ReceivedTasks.Any() && worker.CurrentShift() == null)
                {
                    // Here status of tasks should be checked as well
                    List<KeyValuePair<Models.Entities.Storage.Task, User>> tasksToRemove =
                        new List<KeyValuePair<Models.Entities.Storage.Task, User>>();

                    foreach (var unifinishedTask in worker.ReceivedTasks)
                    {
                        var target = _context.Cases.Find(unifinishedTask.CaseID);
                        var newWorker = _taskDisposer.FindWorker(target).Result;
                        if (newWorker != null)
                        {
                            tasksToRemove
                                .Add(new KeyValuePair<Models.Entities.Storage.Task, User>(unifinishedTask, newWorker));
                        }
                    }

                    foreach (var pair in tasksToRemove)
                    {
                        pair.Value.ReceivedTasks.Add(pair.Key);
                        pair.Key.Receiver = pair.Value;
                        _context.Entry(pair.Value).State = EntityState.Modified;
                        _context.Entry(pair.Key).State = EntityState.Modified;
                        worker.ReceivedTasks.Remove(pair.Key);
                        _context.Entry(worker).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
