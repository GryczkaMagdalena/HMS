using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Entities.Storage
{
    public class Rule
    {
        public Guid RuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
