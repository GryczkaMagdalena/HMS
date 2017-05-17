using HotelManagementSystem.Models.Concrete;
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelManagementSystem.Models.Entities.Storage;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Models.Infrastructure.IdentityBase;

namespace HotelManagementSystem.Models.Infrastructure
{
    public class UserService 
    {
        private ApplicationUserManager _userManager;
        private SignInManager<User> _signInManager;
        private IPasswordHasher<User> _passwordHasher;
        private RoleManager<IdentityRole> _roleManager;

        public async Task<bool> RoleExists(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
        public async Task<SignInResult> PasswordSignInAsync(UserViewModel user, string password)
        {
            return await _signInManager.PasswordSignInAsync(user.Login, password, true, false);
        }
        public UserService(ApplicationUserManager userManager, SignInManager<User> signInManager, IPasswordHasher<User> hasher,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = hasher;
            _roleManager = roleManager;
        }
        public async Task<string> MainRole(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(q => q == "Administrator" || q == "admin"))
            {
                return "Administrator";
            }
            else if(roles.Any(q=>q=="Worker"||q=="worker"))
            {
                return "Worker";
            }
            else
            {
                return "Customer";
            }
        }
        public async Task<IList<string>> GetUserRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> RemoveFromRole(string roleName,String userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> AddUserToRole(string roleName,String UserID)
        {
            var user = await _userManager.FindByIdAsync(UserID);
            return await _userManager.AddToRoleAsync(user, roleName);
        }
        public async Task<IdentityResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return new IdentityResult();
        }
        public async Task<User> GetUserByUsername(string username)
        {
            if (username.Contains("@"))
                return await _userManager.FindByEmailAsync(username);
            return await _userManager.FindByNameAsync(username);
        }


        public async Task<IdentityResult> CreateUser (User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<Room> GetRoomAsync(User user)
        {
           var entity = await _userManager.Users.Include(q => q.Room).FirstAsync(u=>u.Id==user.Id);
            return entity.Room;
        }

        public PasswordVerificationResult VerifyHashedPassword(User user,string password)
        {
            return _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        }
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IList<Claim>> GetClaims(User user)
        {
            return await _userManager.GetClaimsAsync(user);
        }
        
        public async Task<bool> IsInRoleAsync(User guestAccount, string role)
        {
            return await _userManager.IsInRoleAsync(guestAccount, role);
        }

        internal Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            return _userManager.GetUserAsync(user);
        }
    }
}
