using FluentValidation;
using MovieTicketingSystem.Application.Commands.Movies;

namespace MovieTicketingSystem.Application.Validators
{
    public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
    {
        public CreateMovieCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.Genre)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Language)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.DurationInMinutes)
                .GreaterThan(0)
                .LessThanOrEqualTo(600);

            RuleFor(x => x.Director)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Cast)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.PosterUrl)
                .MaximumLength(500);

            RuleFor(x => x.TrailerUrl)
                .MaximumLength(500);

            RuleFor(x => x.ReleaseDate)
                .NotEmpty();
        }
    }
} 