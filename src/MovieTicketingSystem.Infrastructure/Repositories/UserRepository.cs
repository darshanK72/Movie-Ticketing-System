using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MovieTicketingSystem.Application.DTOs.Auth;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Infrastructure.Repositories
{
    public class UserRepository(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration) : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly IConfiguration _configuration = configuration;

        public async Task<bool> RegisterUser(User user) 
        {
            var identityUser = new IdentityUser()
            {
                UserName = user.Email,
                Email = user.Email,
            };

            var result = await _userManager.CreateAsync(user, user.Password!);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("FirstName", user.FirstName));
                await _userManager.AddClaimAsync(user, new Claim("LastName", user.LastName));

                if (!string.IsNullOrEmpty(user.Role.ToString()))
                {
                    if (!await _roleManager.RoleExistsAsync(user.Role.ToString()))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(user.Role.ToString()));
                    }

                    await _userManager.AddToRoleAsync(user, user.Role.ToString());
                }

                return true;
            }

            return false;
        }
    }
}
