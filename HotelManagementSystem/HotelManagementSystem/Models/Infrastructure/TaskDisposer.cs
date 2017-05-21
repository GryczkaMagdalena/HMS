using HotelManagementSystem.Models.Entities.Identity;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Helpers;
using HotelManagementSystem.Models.Abstract;

namespace HotelManagementSystem.Models.Infrastructure
{
    public class TaskDisposer
    {
        private IdentityContext _context;
        private readonly IUserService _userService;
        public TaskDisposer(IUserService userService, IdentityContext context)
        {
            this._userService = userService;
            _context = context;
        }
        private async Task<List<User>> GetWorkersByType(WorkerType type)
        {
            List<User> resultList = new List<User>();
            var users = await _context.LazyLoadUsers();
            foreach (var user in users)
            {
                if (await _userService.IsInRoleAsync(user, "Worker") && user.WorkerType == type)
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        private List<User> GetWorkersWhoCanPerformTask(TimeSpan caseDuration, List<User> workers)
        {
            List<User> workersWhoCanPerformTask = new List<User>();
            foreach (var worker in workers)
            {
                if (TaskNotExceedsWorkingTime(worker, caseDuration))
                {
                    workersWhoCanPerformTask.Add(worker);
                }
            }
            return workersWhoCanPerformTask;
        }
        private bool TaskNotExceedsWorkingTime(User worker, TimeSpan caseDuration)
        {
            Shift shift = WorkerCurrentShift(worker);
            if (DateTime.Now.Add(caseDuration) < shift.EndTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private List<User> GetWorkersInWork(List<User> workers)
        {
            List<User> workersInWork = new List<User>();
            foreach (var worker in workers)
            {
                if (CurrentlyInWork(worker))
                {
                    workersInWork.Add(worker);
                }
            }
            return workersInWork;
        }
        private Shift WorkerCurrentShift(User worker)
        {
            try
            {
                var now = DateTime.Now;
                if (worker.Shifts != null)
                    return worker.Shifts.First(q => q.StartTime < now && q.EndTime > now);
                else
                    return null;
            }catch(Exception ex)
            {
                Console.Write(ex.ToString());
                return null; 
            }
        }
        private bool CurrentlyInWork(User worker)
        {
            var currentShift = WorkerCurrentShift(worker);
            if (currentShift != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CurrentlyHaveBreak(User worker)
        {
            return WorkerCurrentShift(worker).Break;
        }

        private List<User> FilterOnBreak(List<User> workers)
        {
            List<User> NotOnBreak = new List<User>();
            foreach(var worker in workers)
            {
                if (!CurrentlyHaveBreak(worker))
                {
                    NotOnBreak.Add(worker);
                }
            }
            return NotOnBreak;
        }
        public async Task<User> FindWorker(Case toDo)
        {

            List<User> targetWorkers = new List<User>();
            var possibleWorkers = await GetWorkersByType(toDo.WorkerType);
            if (possibleWorkers.Count == 0) return null;

            var workersInWork = GetWorkersInWork(possibleWorkers);
            if (workersInWork.Count == 0) return null;
            workersInWork = FilterOnBreak(workersInWork);

            var workersNotLeaving = GetWorkersWhoCanPerformTask(toDo.EstimatedTime, workersInWork);
            if (workersNotLeaving.Count == 0)
            {
                targetWorkers = workersInWork.Where(p => p.ReceivedTasks != null).OrderBy(q => q.ReceivedTasks.Count).ToList();
               targetWorkers.AddRange(workersNotLeaving.Where(q => q.ReceivedTasks == null));
            }
            else
            {
               targetWorkers = workersNotLeaving.Where(p=>p.ReceivedTasks!=null).OrderBy(q => q.ReceivedTasks.Count).ToList();
               targetWorkers.AddRange(workersNotLeaving.Where(q => q.ReceivedTasks == null));
            }
            return targetWorkers.First();
        }

        public async Task<User> AttachListeningManager(Case toDo, User worker)
        {
            return null;
        }
    }
}
