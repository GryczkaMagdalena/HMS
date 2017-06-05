using FluentScheduler;
using HotelManagementSystem.Models.Abstract;
using HotelManagementSystem.Models.Entities.Storage;
using HotelManagementSystem.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagementSystem.Models.Infrastructure.Scheduler.Jobs
{
    public class MoveOldAssigned : IJob
    {
        private readonly IdentityContext _context;
        private readonly TaskDisposer _taskDisposer;
        private readonly IUserService _userService;
        public MoveOldAssigned(IdentityContext context,IUserService userService)
        {
            _context = context;
            _userService = userService;
            _taskDisposer = new TaskDisposer(userService,context);
        }
        public void Execute()
        {
            List<Task> tasks = _context.LazyLoadTasks().Result
                ?.Where(q => q.Status == Status.Assigned &&
                DateTime.Now > (q.TimeOfCreation.Add(q.Case.EstimatedTime))).ToList();
                
            foreach(var task in tasks)
            {
                var newWorker = _taskDisposer.FindWorker(_context.Cases.Find(task.CaseID)).Result;
                var oldWorker = _context.LazyLoadWorker(task.ReceiverID).Result;
                if (newWorker != null)
                {
                    newWorker.ReceivedTasks.Add(task);
                    oldWorker.ReceivedTasks.Remove(task);
                    _context.Entry(newWorker).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.Entry(oldWorker).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    task.Receiver = newWorker;
                    _context.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }
    }
}
