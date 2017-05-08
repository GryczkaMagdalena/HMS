using HotelManagementSystem.Models.Entities.Identity;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Infrastructure
{
    public class TaskDisposer
    {
        private IdentityContext identityContext;
        private readonly UserService userService;
        public TaskDisposer(UserService userService)
        {
            this.userService = userService;
            identityContext = new IdentityContext();
        }
        private async Task<List<User>> GetWorkersByType(WorkerType type)
        {
            List<User> resultList = new List<User>();
            var users = identityContext.Users.ToList();
            foreach (var user in users)
            {
                if (await userService.IsInRoleAsync(user, "Worker") && user.WorkerType == type)
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
            var now = DateTime.Now;
            if (worker.Shifts != null)
                return worker.Shifts.First(q => q.StartTime < now && q.EndTime > now);
            else
                return null;
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
            //TODO use break field
            return false;
        }

        private List<User> FilterOnBreak(List<User> workers)
        {
            List<User> NotOnBreak = new List<User>();
            foreach(var worker in workers)
            {
                //TODO if worker has breake
                if (false)
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

            //TODO add real timespan of case
            var workersNotLeaving = GetWorkersWhoCanPerformTask(new TimeSpan(), workersInWork);
            if (workersNotLeaving.Count == 0)
            {
               // targetWorkers = workersInWork.OrderBy(q => q.ReceivedTasks.Count).ToList();
            }
            else
            {
               // targetWorkers = workersNotLeaving.OrderBy(q => q.ReceivedTasks.Count).ToList();
            }
            return targetWorkers.First();
        }

        public async Task<User> AttachListeningManager(Case toDo, User worker)
        {
            return new User();
        }
    }
}
