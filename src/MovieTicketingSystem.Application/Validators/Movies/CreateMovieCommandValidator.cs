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

            RuleFor(x => x.GenreIds)
                .NotEmpty()
                .Must(ids => ids != null && ids.Count > 0)
                .WithMessage("At least one genre must be selected");

            RuleFor(x => x.LanguageIds)
                .NotEmpty()
                .Must(ids => ids != null && ids.Count > 0)
                .WithMessage("At least one language must be selected");

            RuleFor(x => x.DurationInMinutes)
                .GreaterThan(0)
                .LessThanOrEqualTo(600);

            RuleFor(x => x.Director)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.PosterUrl)
                .MaximumLength(500);

            RuleFor(x => x.TrailerUrl)
                .MaximumLength(500);

            RuleFor(x => x.ReleaseDate)
                .NotEmpty();

            RuleFor(x => x.CertificateRating)
                .IsInEnum();

            RuleFor(x => x.ViewerRating)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(10)
                .When(x => x.ViewerRating.HasValue);
        }
    }
} 