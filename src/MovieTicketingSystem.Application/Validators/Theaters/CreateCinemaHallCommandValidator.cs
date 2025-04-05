using FluentValidation;
using MovieTicketingSystem.Application.Commands.Theaters;

namespace MovieTicketingSystem.Application.Validators.Theaters
{
    public class CreateCinemaHallCommandValidator : AbstractValidator<CreateCinemaHallCommand>
    {
        public CreateCinemaHallCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Cinema hall name is required")
                .MaximumLength(100).WithMessage("Cinema hall name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.TotalSeats)
                .GreaterThan(0).WithMessage("Total seats must be greater than 0");

            RuleFor(x => x.NumberOfRows)
                .GreaterThan(0).WithMessage("Number of rows must be greater than 0");

            RuleFor(x => x.SeatsPerRow)
                .GreaterThan(0).WithMessage("Seats per row must be greater than 0");

            RuleFor(x => x)
                .Must(x => x.TotalSeats == x.NumberOfRows * x.SeatsPerRow)
                .WithMessage("Total seats must be equal to number of rows multiplied by seats per row");

            RuleFor(x => x.TheaterId)
                .NotEmpty().WithMessage("Theater ID is required");
        }
    }
} 