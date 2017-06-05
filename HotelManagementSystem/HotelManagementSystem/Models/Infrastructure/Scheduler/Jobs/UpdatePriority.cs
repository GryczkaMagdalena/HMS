using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            foreach (var task in _context.LazyLoadTasks().Result.Where(q => q.Priority < Priority.Compulsory)) 
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
