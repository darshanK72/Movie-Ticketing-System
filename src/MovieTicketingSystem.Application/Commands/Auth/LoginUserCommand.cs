using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs.Auth;

namespace MovieTicketingSystem.Application.Commands.Auth
{
    public class LoginUserCommand : IRequest<TokenResponse>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
