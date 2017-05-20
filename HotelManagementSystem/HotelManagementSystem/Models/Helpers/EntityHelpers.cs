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

        public static object ToJson (this User user)
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
        
        public static async  Task<User> LazyLoadUser(this IdentityContext context,string UserID)
        {
            return await context.Users.Include(p => p.Roles).Include(p => p.Shifts)
                .Include(p => p.Room).Include(p => p.IssuedTasks).Include(p => p.ListenedTasks)
                .Include(p => p.ReceivedTasks).FirstAsync(q => q.Id == UserID);
        }
        public static async Task<List<User>> LazyLoadUsers(this IdentityContext context)
        {
            return await context.Users.Include(p => p.Roles).Include(p => p.Shifts)
                .Include(p => p.Room).Include(p => p.IssuedTasks).Include(p => p.ListenedTasks)
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

        public static async Task<User> LazyLoadUserByEmail(this IdentityContext context,string email)
        {
            return await context.Users.Include(p => p.Roles).Include(p => p.Shifts)
              .Include(p => p.Room).Include(p => p.IssuedTasks).Include(p => p.ListenedTasks)
              .Include(p => p.ReceivedTasks).FirstAsync(q => q.Email == email);
        }

        public static async Task<Room> LazyLoadRoom (this IdentityContext context, Guid RoomID)
        {
            return await context.Rooms.Include(q => q.User).FirstAsync(q => q.RoomID == RoomID);
        }

        public static async Task<List<Room>> LazyLoadRooms (this IdentityContext context)
        {
            return await context.Rooms.Include(q => q.User).ToListAsync();
        }
    }
}
