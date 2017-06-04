using HotelManagementSystem.Models.Entities.Identity;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models.Entities.Storage
{
    public class Room
    {
        [Key]
        public Guid RoomID { get; set; }
        public string Number { get; set; }
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        [JsonIgnore]
        public virtual Customer User { get; set; }
        [DefaultValue(false)]
        public bool Occupied { get; set; }
    }
}
