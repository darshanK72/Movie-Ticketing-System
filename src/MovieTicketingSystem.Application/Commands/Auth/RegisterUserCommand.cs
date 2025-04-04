using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MovieTicketingSystem.Application.Commands
{
    public class RegisterUserCommand : IRequest<bool>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
    }
}
