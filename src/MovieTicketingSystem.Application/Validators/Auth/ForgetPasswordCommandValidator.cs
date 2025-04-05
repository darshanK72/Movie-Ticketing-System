using FluentValidation;
using MovieTicketingSystem.Application.Commands.Auth;

namespace MovieTicketingSystem.Application.Validators.Auth
{
    public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required");
        }
    }
} 