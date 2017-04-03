using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models.Entities.Storage
{
    public class Room
    {
        [Key]
        public Guid RoomID { get; set; }
        public string Number { get; set; }
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        [DefaultValue(false)]
        public bool Occupied { get; set; }
    }
}
