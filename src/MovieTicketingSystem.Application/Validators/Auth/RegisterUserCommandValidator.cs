using System;
using FluentValidation;
using MovieTicketingSystem.Application.Commands;
using MovieTicketingSystem.Application.Commands.Auth;

namespace MovieTicketingSystem.Application.Validators.Auth
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

            RuleFor(x => x.Role)
                .Must(BeValidRole).When(x => !string.IsNullOrEmpty(x.Role))
                .WithMessage("Invalid role specified");
        }

        private bool BeValidRole(string role)
        {
            return Enum.TryParse<Domain.Enums.UserRole>(role, out _);
        }
    }
}
