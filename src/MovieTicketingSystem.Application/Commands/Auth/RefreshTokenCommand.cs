using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Commands.Auth
{
    public class RefreshTokenCommand : IRequest<TokenResponse>
    {
        public string? RefreshToken { get; set; }
        public string? Email { get; set; }
    }
}
