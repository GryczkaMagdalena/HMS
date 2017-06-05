using FluentScheduler;
using HotelManagementSystem.Models.Abstract;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using HotelManagementSystem.Models.Helpers;
namespace HotelManagementSystem.Models.Infrastructure.Scheduler.Jobs
{
    public class CollectFinished : IJob
    {
        private readonly IdentityContext _context;
        public CollectFinished(IdentityContext context)
        {
            _context = context;
        }

        public void Execute()
        {
            List<Task> finishedTasks =  _context.LazyLoadTasks().Result.Where(q => q.Status == Status.Done).ToList();
            foreach(var task in finishedTasks)
            {
                var customer = _context.LazyLoadCustomer(task.IssuerID).Result;
                var worker = _context.LazyLoadWorker(task.ReceiverID).Result;
               
                customer.IssuedTasks.Remove(task);
                worker.ReceivedTasks.Remove(task);
                if (task.ListenerID != null)
                {
                    var manager = _context.LazyLoadManager(task.ListenerID).Result;
                    manager.ListenedTasks.Remove(task);
                    _context.Entry(manager).State = EntityState.Modified;
                }
                _context.Entry(task).State = EntityState.Deleted;
                _context.Entry(customer).State = EntityState.Modified;
                _context.Entry(worker).State = EntityState.Modified;
                _context.SaveChanges();

            }
        }
    }
}
