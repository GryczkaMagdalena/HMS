using FluentScheduler;
using HotelManagementSystem.Models.Abstract;
using HotelManagementSystem.Models.Entities.Storage;
using HotelManagementSystem.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagementSystem.Models.Infrastructure.Scheduler.Jobs
{
    public class AssignUnassigned : IJob
    {
        private readonly IdentityContext _context;
        private readonly TaskDisposer _taskDisposer;
        private readonly IUserService _userService;
        public AssignUnassigned(IdentityContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
            _taskDisposer = new TaskDisposer(userService, context);
        }

        public void Execute()
        {
            List<Task> unassignedTasks = _context.LazyLoadTasks().Result?.Where(q => q.Status == Status.Unassigned).ToList();
            foreach(var task in unassignedTasks)
            {
                var newWorker = _taskDisposer.FindWorker(_context.Cases.Find(task.CaseID)).Result;
                if (newWorker != null)
                {
                    newWorker.ReceivedTasks.Add(task);
                    task.Receiver = newWorker;
                    task.Priority = Priority.High;
                    task.Status = Status.Assigned;
                    _context.Entry(newWorker).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
            }

        }
    }
}
