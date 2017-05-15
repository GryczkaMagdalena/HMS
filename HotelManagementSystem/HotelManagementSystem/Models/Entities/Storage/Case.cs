using HotelManagementSystem.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Entities.Storage
{
    public class Case
    {
        [Key]
        public Guid CaseID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkerType WorkerType { get; set; }
        public TimeSpan EstimatedTime { get; set; }
    }
}
