using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MediatR;
using MovieTicketingSystem.Application.Commands.Auth;
using MovieTicketingSystem.Domain.Contracts.Repository;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        
        public AuthController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (result)
                return Ok(new { Message = "User registered successfully." });
                
            return BadRequest(new { Message = "Registration failed." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var tokenResponse = await _mediator.Send(command);

            if(tokenResponse != null)
            {
                return Ok(new { Result = tokenResponse, Message = "User logged in successfully." });
            }
            
            return Unauthorized(new { Message = "Invalid login attempt." });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Login([FromBody] LogoutUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(new {Message = "User logged out successfully." });
            }

            return Unauthorized(new { Message = "Invalid login attempt." });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if(result != null)
            {
                return Ok(new { Result = result , Message = "If your email is registered, you will receive a password reset link." });
            }

            return BadRequest(new { Message = "This email is not registered, please register first." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
           var result = await _mediator.Send(command);
            
            if (result)
                return Ok(new { Message = "Password has been reset successfully." });
                
            return BadRequest(new { Message = "Password reset failed. The token may be invalid or expired." });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var tokenResponse = await _mediator.Send(command);

            if(tokenResponse != null)
            {
                return Ok(new {Result = tokenResponse, Message = "Token refreshed successfully."});
            }
            
            return Unauthorized(new { Message = "Invalid refresh token." });
        }
    }
} 