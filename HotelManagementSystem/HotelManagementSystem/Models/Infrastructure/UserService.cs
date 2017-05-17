using HotelManagementSystem.Models.Concrete;
using HotelManagementSystem.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

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

        internal Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            return _userManager.GetUserAsync(user);
        }
    }

    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
    }
    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {
    }
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
    }

    public class ApplicationUserStore : UserStore<User, IdentityRole, IdentityContext, string>
    {
        public ApplicationUserStore(IdentityContext context, IdentityErrorDescriber describer =null)
            :base(context, describer)
        {

        }
    }

    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, 
            IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) 
            : base(store, optionsAccessor, passwordHasher, 
                  userValidators, passwordValidators, 
                  keyNormalizer, errors, services, logger)
        {
        }
    }

}
