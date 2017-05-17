using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Infrastructure.IdentityBase
{
    public class ApplicationUserStore : UserStore<User, IdentityRole, IdentityContext, string>
    {
        public ApplicationUserStore(IdentityContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {

        }
    }
}
