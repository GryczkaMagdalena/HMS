using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models.Entities.Storage
{
    public class Shift
    {
        public Guid ShiftID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan ActualTime { get; set; }
        [DefaultValue(false)]
        public Boolean Break { get; set; }
        public string UserID { get; set; }
    }
}
