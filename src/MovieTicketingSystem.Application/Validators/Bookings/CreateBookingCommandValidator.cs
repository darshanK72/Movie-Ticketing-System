using FluentValidation;
using MovieTicketingSystem.Application.Commands.Bookings;

namespace MovieTicketingSystem.Application.Validators.Bookings
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");

            RuleFor(x => x.ShowId)
                .NotEmpty()
                .WithMessage("Show ID is required");

            RuleFor(x => x.SeatIds)
                .NotEmpty()
                .WithMessage("At least one seat must be selected");

            RuleFor(x => x.NumberOfTickets)
                .GreaterThan(0)
                .WithMessage("Number of tickets must be greater than 0");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0)
                .WithMessage("Total amount must be greater than 0");
        }
    }
} 