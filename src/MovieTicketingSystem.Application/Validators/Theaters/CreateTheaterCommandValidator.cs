using FluentValidation;
using MovieTicketingSystem.Application.Commands.Theaters;

namespace MovieTicketingSystem.Application.Validators.Theaters
{
    public class CreateTheaterCommandValidator : AbstractValidator<CreateTheaterCommand>
    {
        public CreateTheaterCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Theater name is required")
                .MaximumLength(100).WithMessage("Theater name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.ContactNumber)
                .NotEmpty().WithMessage("Contact number is required")
                .MaximumLength(20).WithMessage("Contact number cannot exceed 20 characters")
                .Matches(@"^\+?[\d\s-]+$").WithMessage("Invalid contact number format");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.Website)
                .MaximumLength(200).WithMessage("Website URL cannot exceed 200 characters")
                .Matches(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$")
                .When(x => !string.IsNullOrEmpty(x.Website))
                .WithMessage("Invalid website URL format");

            RuleFor(x => x.AddressDetails)
                .MaximumLength(500).WithMessage("Address details cannot exceed 500 characters");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required")
                .MaximumLength(200).WithMessage("Street cannot exceed 200 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required")
                .MaximumLength(100).WithMessage("State cannot exceed 100 characters");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Postal code is required")
                .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters")
                .Matches(@"^[0-9]{5,6}$").WithMessage("Invalid postal code format");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters");
        }
    }
} 