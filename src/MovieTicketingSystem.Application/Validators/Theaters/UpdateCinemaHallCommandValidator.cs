using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MovieTicketingSystem.Application.Commands.Theaters;

namespace MovieTicketingSystem.Application.Validators.Theaters
{
    public class UpdateCinemaHallCommandValidator : AbstractValidator<UpdateCinemaHallCommand>
    {
        public UpdateCinemaHallCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Cinema hall ID is required");

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Cinema hall name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.TotalSeats)
                .GreaterThan(0).WithMessage("Total seats must be greater than 0")
                .When(x => x.TotalSeats.HasValue);

            RuleFor(x => x.NumberOfRows)
                .GreaterThan(0).WithMessage("Number of rows must be greater than 0")
                .When(x => x.NumberOfRows.HasValue);

            RuleFor(x => x.SeatsPerRow)
                .GreaterThan(0).WithMessage("Seats per row must be greater than 0")
                .When(x => x.SeatsPerRow.HasValue);

            RuleFor(x => x)
                .Must(x => !x.TotalSeats.HasValue || !x.NumberOfRows.HasValue || !x.SeatsPerRow.HasValue || 
                          x.TotalSeats == x.NumberOfRows * x.SeatsPerRow)
                .WithMessage("Total seats must be equal to number of rows multiplied by seats per row");
        }
    }
}
