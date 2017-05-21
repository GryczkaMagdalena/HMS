using HotelManagementSystem.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using HotelManagementSystem.Models.Concrete;
using HotelManagementSystem.Models.Entities.Identity;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Tests.Infrastructure
{
    public class MockUserService : IUserService
    {
        public Task<IdentityResult> AddUserToRole(string roleName, string UserID)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateUser(User user, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Claim>> GetClaims(User user)
        {
            return new List<Claim>();
        }

        public async Task<Room> GetRoomAsync(User user)
        {
            using (var context = new IdentityContext())
            {
                if (user.RoomID != null)
                {
                    var room = await context.Rooms.FindAsync(user.RoomID);
                    return room;
                }
                else
                {
                    return null;
                }
            }
        }

        public Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            using (var context = new IdentityContext())
            {
                if(username.Contains("@"))
                 return  await context.Users.Include(q=>q.Roles).FirstAsync(p => p.Email == username);
                return await context.Users.Include(q=>q.Roles).FirstAsync(q => q.UserName == username);
            }
        }

        public async Task<IList<string>> GetUserRoles(User user)
        {
            using (var context = new IdentityContext())
            {
                List<string> userRoles = new List<string>();
                foreach(var userRole in user.Roles)
                {
                    var role = await context.Roles.FindAsync(userRole.RoleId);
                    userRoles.Add(role.Name);
                }
                return userRoles;
            }
        }

        public Task<bool> IsInRoleAsync(User guestAccount, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<string> MainRole(User user)
        {
           using (var context = new IdentityContext())
            {
                if (user.Roles != null)
                {
                    var role = await context.Roles.FindAsync(user.Roles.First().RoleId);
                    return role.Name;
                }
                else
                {
                    return null;
                }
            }
        }

        public Task<SignInResult> PasswordSignInAsync(UserViewModel user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> RemoveFromRole(string roleName, string userID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> SignOut()
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string password)
        {
            return PasswordVerificationResult.Success;
        }
    }
}
