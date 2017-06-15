using HotelManagementSystem.Models.Entities.Identity;
using HotelManagementSystem.Models.Entities.Storage;
using HotelManagementSystem.Models.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Helpers
{
    public static class EntityHelpers
    {
        public static string ToString(this Shift shift)
        {
            return shift.StartTime.ToString("dd/MM HH:mm") + " -> " + shift.EndTime.ToString("dd/MM HH:mm"); 
        }

        public static object ToJson (this Worker user)
        {
            if (user == null) return null;
            return new
            {
                UserID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                WorkerType = Enum.GetName(typeof(WorkerType), user.WorkerType),
            };
        }

        public static object ToJson(this User user)
        {
            if (user == null) return null;
            return new
            {
                UserID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
        }

        public static object ToJson(this Customer user)
        {
            if (user == null) return null;
            return new
            {
                UserID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Room = user.Room
            };
        }

        public static async Task<Worker> LazyLoadWorkerByEmail(this IdentityContext context, string email)
        {
            return await context.Workers.Include(q => q.Roles).Include(q => q.Shifts).Include(q => q.ReceivedTasks)
                .FirstAsync(q => q.Email == email);
        }

        public static async Task<Worker> LazyLoadWorker(this IdentityContext context, string UserID)
        {

            return await context.Workers.Include(p => p.Roles).Include(p => p.Shifts)
              .Include(p => p.ReceivedTasks).FirstAsync(q => q.Id == UserID);
        }

        public static async Task<Customer> LazyLoadCustomer (this IdentityContext context, string UserID)
        {
            return await context.Customers.Include(p => p.Roles)
               .Include(p => p.Room).Include(p => p.IssuedTasks)
               .FirstAsync(q => q.Id == UserID);
        }

        public static async  Task<Manager> LazyLoadManager(this IdentityContext context,string UserID)
        {
            return await context.Managers.Include(p => p.Roles).Include(p => p.Shifts)
                .Include(p => p.ListenedTasks)
                .FirstAsync(q => q.Id == UserID);
        }
        public static async Task<List<Worker>> LazyLoadWorkers(this IdentityContext context)
        {
            return await context.Workers.Include(p => p.Roles).Include(p => p.Shifts)
                .Include(p => p.ReceivedTasks).ToListAsync();
        }

        public static async Task<Entities.Storage.Task> LazyLoadTask(this IdentityContext context,Guid TaskID)
        {
            return await context.Tasks.Include(q => q.Case).Include(q => q.Issuer).Include(q => q.Listener)
                .Include(q => q.Receiver).Include(q => q.Room).FirstAsync(q => q.TaskID == TaskID);
        }

        public static async Task<List<Entities.Storage.Task>> LazyLoadTasks(this IdentityContext context)
        {
            return await context.Tasks.Include(q => q.Case).Include(q => q.Issuer).Include(q => q.Listener)
                .Include(q => q.Receiver).Include(q => q.Room).ToListAsync();
        }

        public static async Task<Customer> LazyLoadUserByEmail(this IdentityContext context,string email)
        {
            return await context.Customers.Include(p => p.Roles).Include(q=>q.Room).Include(q=>q.IssuedTasks)
                .FirstAsync(q => q.Email == email);
        }

        public static async Task<Room> LazyLoadRoom (this IdentityContext context, Guid RoomID)
        {
            return await context.Rooms.Include(q => q.User).FirstAsync(q => q.RoomID == RoomID);
        }

        public static async Task<List<Room>> LazyLoadRooms (this IdentityContext context)
        {
            return await context.Rooms.Include(q => q.User).ToListAsync();
        }

        public static Shift CurrentShift (this Worker worker)
        {
            try
            {
                var now = DateTime.Now;
                if (worker.Shifts != null)
                    return worker.Shifts.First(q => q.StartTime < now && q.EndTime > now);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
