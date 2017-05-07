using HotelManagementSystem.Models.Concrete;
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models.Infrastructure
{
    public class UserService
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IPasswordHasher<User> _passwordHasher;
        private RoleManager<Role> _roleManager;

        public async Task<bool> RoleExists(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
        public async Task<SignInResult> PasswordSignInAsync(UserViewModel user, string password)
        {
            return await _signInManager.PasswordSignInAsync(user.Login, password, true, false);
        }
        public UserService(IdentityContext context,UserManager<User> userManager, SignInManager<User> signInManager, IPasswordHasher<User> hasher,RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = hasher;
            _roleManager = roleManager;
        }
        public async Task<IList<string>> GetUserRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddUserToRole(string roleName,User user)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }
        public async Task<IdentityResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return new IdentityResult();
        }
        public async Task<User> GetUserByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }


        public async Task<IdentityResult> CreateUser (User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public PasswordVerificationResult VerifyHashedPassword(User user,string password)
        {
            return _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        }

        public async Task<IList<Claim>> GetClaims(User user)
        {
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<bool> IsInRoleAsync(User guestAccount, string role)
        {
            return await _userManager.IsInRoleAsync(guestAccount, role);
        }
    }
}
