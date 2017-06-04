using HotelManagementSystem.Models.Abstract;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HotelManagementSystem.Models.Entities.Identity
{
    public class User : IdentityUser
    {
        [JsonProperty(PropertyName ="firstName")]
        [JsonRequired]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName ="lastName")]
        [JsonRequired]
        public string LastName { get; set; }
    }
    
}
