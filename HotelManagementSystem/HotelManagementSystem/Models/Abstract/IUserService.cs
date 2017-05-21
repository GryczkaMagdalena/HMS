using HotelManagementSystem.Models.Concrete;
using HotelManagementSystem.Models.Entities.Identity;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Abstract
{
    public interface IUserService 
    {
        Task<bool> RoleExists(string roleName);
        Task<SignInResult> PasswordSignInAsync(UserViewModel user, string password);
        Task<string> MainRole(User user);
        Task<IdentityResult> AddUserToRole(string roleName, string UserID);
        Task<IdentityResult> RemoveFromRole(string roleName, string userID);
        Task<IdentityResult> SignOut();
        Task<User> GetUserByUsername(string username);
        Task<IdentityResult> CreateUser(User user, string password);
        Task<Room> GetRoomAsync(User user);
        PasswordVerificationResult VerifyHashedPassword(User user, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IList<Claim>> GetClaims(User user);
        Task<bool> IsInRoleAsync(User guestAccount, string role);
        Task<User> GetUserAsync(ClaimsPrincipal user);
        Task<IList<string>> GetUserRoles(User user);
    }
}
