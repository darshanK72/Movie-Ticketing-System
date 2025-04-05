using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MovieTicketingSystem.Application.Commands.Auth
{
    public class LogoutUserCommand : IRequest<bool>
    {
        public string? Email { get; set; }
    }
}
