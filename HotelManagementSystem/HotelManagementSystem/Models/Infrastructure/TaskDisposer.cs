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
        private async Task<List<Worker>> GetWorkersByType(WorkerType type)
        {
            List<Worker> resultList = new List<Worker>();
            var users = await _context.LazyLoadWorkers();
            foreach (var user in users)
            {
                if (user.WorkerType == type)
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        private List<Worker> GetWorkersWhoCanPerformTask(TimeSpan caseDuration, List<Worker> workers)
        {
            List<Worker> workersWhoCanPerformTask = new List<Worker>();
            foreach (var worker in workers)
            {
                if (TaskNotExceedsWorkingTime(worker, caseDuration))
                {
                    workersWhoCanPerformTask.Add(worker);
                }
            }
            return workersWhoCanPerformTask;
        }
        private bool TaskNotExceedsWorkingTime(Worker worker, TimeSpan caseDuration)
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
        private List<Worker> GetWorkersInWork(List<Worker> workers)
        {
            List<Worker> workersInWork = new List<Worker>();
            foreach (var worker in workers)
            {
                if (CurrentlyInWork(worker))
                {
                    workersInWork.Add(worker);
                }
            }
            return workersInWork;
        }
        private Shift WorkerCurrentShift(Worker worker)
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
        private bool CurrentlyInWork(Worker worker)
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

        private bool CurrentlyHaveBreak(Worker worker)
        {
            return WorkerCurrentShift(worker).Break;
        }

        private List<Worker> FilterOnBreak(List<Worker> workers)
        {
            List<Worker> NotOnBreak = new List<Worker>();
            foreach(var worker in workers)
            {
                if (!CurrentlyHaveBreak(worker))
                {
                    NotOnBreak.Add(worker);
                }
            }
            return NotOnBreak;
        }
        public async Task<Worker> FindWorker(Case toDo)
        {

            List<Worker> targetWorkers = new List<Worker>();
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

        public async Task<Manager> AttachListeningManager(Case toDo, Worker worker)
        {
            return null;
        }
    }
}
