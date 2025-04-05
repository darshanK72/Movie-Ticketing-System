using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.DTOs.Auth;

namespace MovieTicketingSystem.Domain.Contracts.Repository
{
    public interface IUserRepository
    {
        Task<bool> RegisterUser(User user);
        Task<TokenResponse?> LoginUser(string email, string password);
        Task<string?> ForgotPassword(string email);
        Task<bool> ResetPassword(string email, string token, string newPassword);
        Task<TokenResponse?> RefreshToken(string refreshToken, string email);
    }
}
