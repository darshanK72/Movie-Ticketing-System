using FluentValidation;
using MovieTicketingSystem.Application.Commands.Shows;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Validators.Shows
{
    public class UpdateShowCommandValidator : AbstractValidator<UpdateShowCommand>
    {
        public UpdateShowCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Show ID is required");

            RuleFor(x => x.MovieId)
                .NotEmpty()
                .WithMessage("Movie ID is required");

            RuleFor(x => x.CinemaHallId)
                .NotEmpty()
                .WithMessage("Cinema Hall ID is required");

            RuleFor(x => x.ShowTimings)
                .NotEmpty()
                .WithMessage("At least one show timing is required");

            RuleForEach(x => x.ShowTimings).SetValidator(new UpdateShowTimingDetailsValidator());
        }
    }

    public class UpdateShowTimingDetailsValidator : AbstractValidator<UpdateShowTimingDetails>
    {
        public UpdateShowTimingDetailsValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Show Timing ID is required");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required")
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                .WithMessage("Show date must be in the future");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithMessage("End time is required")
                .Must((timing, endTime) => endTime > timing.StartTime)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.BasePrice)
                .GreaterThan(0)
                .WithMessage("Base price must be greater than 0");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid show status");
        }
    }
} 