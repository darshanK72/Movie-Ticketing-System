using FluentValidation;
using MovieTicketingSystem.Application.Commands;
using MovieTicketingSystem.Application.Commands.Auth;

namespace MovieTicketingSystem.Application.Validators.Auth
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
} 