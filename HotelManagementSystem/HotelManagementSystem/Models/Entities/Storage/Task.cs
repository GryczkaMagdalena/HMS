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
        [ForeignKey("RoomID")]
        public Guid RoomID { get; set; }
        public virtual Room Room { get; set; }
    }
}