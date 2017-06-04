using HotelManagementSystem.Models.Entities.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Entities.Identity
{
    [Table("Managers")]
    public class Manager : User
    {
        [InverseProperty("Listener")]
        public virtual List<Storage.Task> ListenedTasks { get; set; }
        public virtual List<Shift> Shifts { get; set; }
    }
}
