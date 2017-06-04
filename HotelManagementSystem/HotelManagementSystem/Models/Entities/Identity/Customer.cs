using HotelManagementSystem.Models.Entities.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Entities.Identity
{
    [Table("Customers")]
    public class Customer : User 
    {
        public Guid? RoomID { get; set; }
        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }

        [InverseProperty("Issuer")]
        public virtual List<Storage.Task> IssuedTasks { get; set; }
    }
}
