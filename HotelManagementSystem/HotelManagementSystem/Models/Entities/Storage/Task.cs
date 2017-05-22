using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelManagementSystem.Models.Entities.Identity;
using System.ComponentModel;

namespace HotelManagementSystem.Models.Entities.Storage
{
    public class Task
    {
        [Key]
        public Guid TaskID { get; set; }
        public string Describe { get; set; }
        public Guid RoomID { get; set; }
        public virtual Room Room { get; set; }
        public string IssuerID { get; set; }
        public virtual User Issuer { get; set; }
        public string ListenerID { get; set; }
        public virtual User Listener { get; set; }
        public string ReceiverID { get; set; }
        public virtual User Receiver { get; set; }
        public virtual Case Case { get; set; }
        public Guid CaseID { get; set; }
        public DateTime TimeOfCreation { get; set; }
        [DefaultValue(Priority.Medium)]
        public Priority Priority { get; set; }
    }

    public enum Priority
    {
        Emergency,
        Compulsory,
        High,
        Medium,
        Low
    }
}