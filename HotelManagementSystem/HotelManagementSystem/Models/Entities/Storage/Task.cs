using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelManagementSystem.Models.Entities.Identity;

namespace HotelManagementSystem.Models.Entities.Storage
{
    public class Task
    {
        [Key]
        public Guid TaskID { get; set; }
        public string Describe { get; set; }
        public virtual Room Room { get; set; }
        public virtual User Issuer { get; set; }
        public virtual User Listener { get; set; }
        public virtual User Receiver { get; set; }
    }
}