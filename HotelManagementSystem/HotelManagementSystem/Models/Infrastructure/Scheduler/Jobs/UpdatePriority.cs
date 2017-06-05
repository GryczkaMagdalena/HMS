using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using HotelManagementSystem.Models.Helpers;
using HotelManagementSystem.Models.Entities.Storage;

namespace HotelManagementSystem.Models.Infrastructure.Scheduler.Jobs
{
    public class UpdatePriority : IJob
    {
        private readonly IdentityContext _context;
        public UpdatePriority(IdentityContext context)
        {
            _context = context;
        }

        public void Execute()
        {
            List<Task> tasks = _context.LazyLoadTasks().Result?.Where(q => q.Priority > Priority.Compulsory).ToList();
            foreach (var task in tasks) 
            {
                TimeSpan timeFromStart = DateTime.Now - task.TimeOfCreation;
                if (timeFromStart > task.Case.EstimatedTime.Add(task.Case.EstimatedTime))
                {
                    task.Priority = Entities.Storage.Priority.Compulsory;
                }
                else
                if (timeFromStart > task.Case.EstimatedTime)
                {
                    task.Priority = Entities.Storage.Priority.High;
                }
                else continue;

                _context.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.ChangeTracker.DetectChanges();
                _context.SaveChanges();
            }
        }
    }
}
