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
                .NotEmpty().WithMessage("Show ID is required");

            RuleFor(x => x.MovieId)
                .NotEmpty().WithMessage("Movie ID is required");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Show date is required")
                .Must(date => date.Date >= DateTime.Today)
                .WithMessage("Show date cannot be in the past");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required")
                .Must((command, endTime) => endTime > command.StartTime)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.CinemaHallId)
                .NotEmpty().WithMessage("Cinema hall ID is required");

            RuleFor(x => x.ShowManagerId)
                .NotEmpty().WithMessage("Show manager ID is required");

            RuleFor(x => x.BasePrice)
                .GreaterThan(0).WithMessage("Base price must be greater than 0");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid show status");
        }
    }
} 