using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Entities.Identity
{
    public class User : IdentityUser
    {
        public override string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [ForeignKey("RoomID")]
        public Guid? RoomID { get; set; }
        public virtual Room Room { get; set; }
        [DefaultValue(2)]
        public WorkerType? WorkerType { get; set; }
        
    }
    public  enum WorkerType
    {
        Cleaner,Technician,None
    }
}
