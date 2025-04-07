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

            RuleFor(x => x.ShowTimingId)
                .NotEmpty()
                .WithMessage("Show ID is required");

            RuleFor(x => x.SeatIds)
                .NotEmpty()
                .WithMessage("At least one seat must be selected");
        }
    }
} 