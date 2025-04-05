using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;
using System.Text.Json;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public UserRepository(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUser(User user)
        {
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

        public async Task<TokenResponse?> LoginUser(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                
                var token = await _userManager.GenerateUserTokenAsync(user, "Default", "Login");
                var refreshToken = await _userManager.GenerateUserTokenAsync(user, "Default", "RefreshToken");

                return new TokenResponse()
                {
                    AccessToken = token,
                    RefreshToken = refreshToken,
                    ExpiresIn = 3600
                };
            }
            return null;
        }

        public async Task<bool> LogoutUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            await _userManager.UpdateSecurityStampAsync(user);

            return true;
        }

        public async Task<string?> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<bool> ResetPassword(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task<TokenResponse?> RefreshToken(string refreshToken, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            { 
                return null;
            }

            var isValid = await _userManager.VerifyUserTokenAsync(user, "Default", "RefreshToken", refreshToken);
            if (isValid)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                
                var newToken = await _userManager.GenerateUserTokenAsync(user, "Default", "Login");
                var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "Default", "RefreshToken");

                return new TokenResponse()
                {
                    AccessToken = newToken,
                    RefreshToken = newRefreshToken,
                    ExpiresIn = 3600
                };
            }
            return null;
        }
    }
}
