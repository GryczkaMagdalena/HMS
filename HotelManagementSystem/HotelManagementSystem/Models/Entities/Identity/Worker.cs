using HotelManagementSystem.Models.Entities.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Entities.Identity
{
    [Table("Workers")]
    public class Worker : User 
    {
        public WorkerType WorkerType { get; set; }
        [InverseProperty("Receiver")]
        public virtual List<Storage.Task> ReceivedTasks { get; set; }
        public virtual List<Shift> Shifts { get; set; }
    }
    public enum WorkerType
    {
        Cleaner, Technician
    }
}
