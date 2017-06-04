using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Entities.Identity
{
    [Table("Administrators")]
    public class Administrator : User
    {
    }
}
