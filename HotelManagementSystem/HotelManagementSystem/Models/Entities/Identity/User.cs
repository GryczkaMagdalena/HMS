using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HotelManagementSystem.Models.Entities.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? RoomID { get; set; }
        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }
        [DefaultValue(2)]
        public WorkerType? WorkerType { get; set; }
        [InverseProperty("Issuer")]
        public virtual List<Storage.Task> IssuedTasks { get; set; }
        [InverseProperty("Listener")]
        public virtual List<Storage.Task> ListenedTasks { get; set; }
        [InverseProperty("Receiver")]
        public virtual List<Storage.Task> ReceivedTasks { get; set; }
        public virtual List<Shift> Shifts { get; set; }
    }
    public  enum WorkerType
    {
        Cleaner,Technician,None
    }
}
