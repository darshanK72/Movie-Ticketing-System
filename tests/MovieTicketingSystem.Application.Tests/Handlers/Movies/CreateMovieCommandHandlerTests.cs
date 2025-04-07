using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using MovieTicketingSystem.Application.Commands.Movies;
using MovieTicketingSystem.Application.Handlers.Movies;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;
using Xunit;

namespace MovieTicketingSystem.Application.Tests.Handlers.Movies.Commands
{
    public class CreateMovieCommandHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IValidator<CreateMovieCommand>> _validatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateMovieCommandHandler _handler;

        public CreateMovieCommandHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _validatorMock = new Mock<IValidator<CreateMovieCommand>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateMovieCommandHandler(
                _movieRepositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesMovie()
        {
            // Arrange
            var command = new CreateMovieCommand
            {
                Title = "Test Movie",
                Description = "Test Description",
                DurationInMinutes = 120,
                Director = "Test Director",
                ReleaseDate = DateTime.UtcNow,
                CertificateRating = CertificateRating.UA,
                ViewerRating = 4.5,
                GenreIds = new List<string> { "genre1", "genre2" },
                LanguageIds = new List<string> { "lang1", "lang2" },
                PosterUrl = "https://example.com/poster.jpg",
                TrailerUrl = "https://example.com/trailer.mp4"
            };

            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                Description = command.Description,
                DurationInMinutes = command.DurationInMinutes,
                Director = command.Director,
                ReleaseDate = command.ReleaseDate,
                CertificateRating = command.CertificateRating,
                ViewerRating = command.ViewerRating,
                PosterUrl = command.PosterUrl,
                TrailerUrl = command.TrailerUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mapperMock
                .Setup(x => x.Map<Movie>(command))
                .Returns(movie);

            _movieRepositoryMock
                .Setup(x => x.CreateMovieAsync(movie, command.GenreIds, command.LanguageIds))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Movie>(command), Times.Once);
            _movieRepositoryMock.Verify(x => x.CreateMovieAsync(movie, command.GenreIds, command.LanguageIds), Times.Once);
        }

        [Fact]
        public async Task Handle_EmptyTitle_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateMovieCommand
            {
                Title = "", // Invalid title
                Description = "Test Description",
                DurationInMinutes = 120,
                Director = "Test Director",
                ReleaseDate = DateTime.UtcNow,
                CertificateRating = CertificateRating.UA,
                ViewerRating = 4.5,
                GenreIds = new List<string> { "genre1" },
                LanguageIds = new List<string> { "lang1" }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Title", "Title is required"));

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Movie>(command), Times.Never);
            _movieRepositoryMock.Verify(x => x.CreateMovieAsync(It.IsAny<Movie>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_EmptyGenreIds_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateMovieCommand
            {
                Title = "Test Movie",
                Description = "Test Description",
                DurationInMinutes = 120,
                Director = "Test Director",
                ReleaseDate = DateTime.UtcNow,
                CertificateRating = CertificateRating.UA,
                ViewerRating = 4.5,
                GenreIds = new List<string>(), // Empty genre list
                LanguageIds = new List<string> { "lang1" }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("GenreIds", "At least one genre must be selected"));

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Movie>(command), Times.Never);
            _movieRepositoryMock.Verify(x => x.CreateMovieAsync(It.IsAny<Movie>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidDuration_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateMovieCommand
            {
                Title = "Test Movie",
                Description = "Test Description",
                DurationInMinutes = 0, // Invalid duration
                Director = "Test Director",
                ReleaseDate = DateTime.UtcNow,
                CertificateRating = CertificateRating.UA,
                ViewerRating = 4.5,
                GenreIds = new List<string> { "genre1" },
                LanguageIds = new List<string> { "lang1" }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("DurationInMinutes", "Duration must be greater than 0"));

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Movie>(command), Times.Never);
            _movieRepositoryMock.Verify(x => x.CreateMovieAsync(It.IsAny<Movie>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidViewerRating_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateMovieCommand
            {
                Title = "Test Movie",
                Description = "Test Description",
                DurationInMinutes = 120,
                Director = "Test Director",
                ReleaseDate = DateTime.UtcNow,
                CertificateRating = CertificateRating.UA,
                ViewerRating = 11, // Invalid rating (should be <= 10)
                GenreIds = new List<string> { "genre1" },
                LanguageIds = new List<string> { "lang1" }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("ViewerRating", "Viewer rating must be between 0 and 10"));

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Movie>(command), Times.Never);
            _movieRepositoryMock.Verify(x => x.CreateMovieAsync(It.IsAny<Movie>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryFailure_ReturnsFalse()
        {
            // Arrange
            var command = new CreateMovieCommand
            {
                Title = "Test Movie",
                Description = "Test Description",
                DurationInMinutes = 120,
                Director = "Test Director",
                ReleaseDate = DateTime.UtcNow,
                CertificateRating = CertificateRating.UA,
                ViewerRating = 4.5,
                GenreIds = new List<string> { "genre1" },
                LanguageIds = new List<string> { "lang1" }
            };

            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                Description = command.Description,
                DurationInMinutes = command.DurationInMinutes,
                Director = command.Director,
                ReleaseDate = command.ReleaseDate,
                CertificateRating = command.CertificateRating,
                ViewerRating = command.ViewerRating,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mapperMock
                .Setup(x => x.Map<Movie>(command))
                .Returns(movie);

            _movieRepositoryMock
                .Setup(x => x.CreateMovieAsync(movie, command.GenreIds, command.LanguageIds))
                .ReturnsAsync(false); // Repository returns false

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Movie>(command), Times.Once);
            _movieRepositoryMock.Verify(x => x.CreateMovieAsync(movie, command.GenreIds, command.LanguageIds), Times.Once);
        }
    }
} 