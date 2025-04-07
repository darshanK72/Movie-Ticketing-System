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
    public class UpdateMovieCommandHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IValidator<UpdateMovieCommand>> _validatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateMovieCommandHandler _handler;

        public UpdateMovieCommandHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _validatorMock = new Mock<IValidator<UpdateMovieCommand>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateMovieCommandHandler(
                _movieRepositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesMovie()
        {
            // Arrange
            var movieId = Guid.NewGuid().ToString();
            var existingMovie = new Movie
            {
                Id = Guid.Parse(movieId),
                Title = "Original Title",
                Description = "Original Description",
                DurationInMinutes = 100,
                Director = "Original Director",
                ReleaseDate = DateTime.UtcNow.AddDays(-30),
                CertificateRating = CertificateRating.U,
                ViewerRating = 3.5,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-60)
            };

            var command = new UpdateMovieCommand
            {
                Id = movieId,
                Title = "Updated Title",
                Description = "Updated Description",
                DurationInMinutes = 120,
                Director = "Updated Director",
                ReleaseDate = DateTime.UtcNow,
                CertificateRating = CertificateRating.UA,
                ViewerRating = 4.5,
                GenreIds = new List<string> { "genre1", "genre2" },
                LanguageIds = new List<string> { "lang1", "lang2" },
                PosterUrl = "https://example.com/poster.jpg",
                TrailerUrl = "https://example.com/trailer.mp4"
            };

            var updatedMovie = new Movie
            {
                Id = Guid.Parse(movieId),
                Title = command.Title,
                Description = command.Description,
                DurationInMinutes = command.DurationInMinutes,
                Director = command.Director,
                ReleaseDate = command.ReleaseDate,
                CertificateRating = command.CertificateRating,
                ViewerRating = command.ViewerRating,
                PosterUrl = command.PosterUrl,
                TrailerUrl = command.TrailerUrl,
                IsActive = existingMovie.IsActive,
                CreatedAt = existingMovie.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _movieRepositoryMock
                .Setup(x => x.GetMovieByIdAsync(movieId))
                .ReturnsAsync(existingMovie);

            _mapperMock
                .Setup(x => x.Map<Movie>(command))
                .Returns(updatedMovie);

            _movieRepositoryMock
                .Setup(x => x.UpdateMovieAsync(updatedMovie, command.GenreIds, command.LanguageIds))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _movieRepositoryMock.Verify(x => x.GetMovieByIdAsync(movieId), Times.Once);
            _mapperMock.Verify(x => x.Map<Movie>(command), Times.Once);
            _movieRepositoryMock.Verify(x => x.UpdateMovieAsync(updatedMovie, command.GenreIds, command.LanguageIds), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var movieId = Guid.NewGuid().ToString();
            var command = new UpdateMovieCommand
            {
                Id = movieId,
                Title = "", // Invalid title
                Description = "Updated Description",
                DurationInMinutes = 120,
                Director = "Updated Director",
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
            _movieRepositoryMock.Verify(x => x.GetMovieByIdAsync(movieId), Times.Never);
            _mapperMock.Verify(x => x.Map<Movie>(command), Times.Never);
            _movieRepositoryMock.Verify(x => x.UpdateMovieAsync(It.IsAny<Movie>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_NonExistentMovie_ReturnsFalse()
        {
            // Arrange
            var movieId = Guid.NewGuid().ToString();
            var command = new UpdateMovieCommand
            {
                Id = movieId,
                Title = "Updated Title",
                Description = "Updated Description",
                DurationInMinutes = 120,
                Director = "Updated Director",
                ReleaseDate = DateTime.UtcNow,
                CertificateRating = CertificateRating.UA,
                ViewerRating = 4.5,
                GenreIds = new List<string> { "genre1" },
                LanguageIds = new List<string> { "lang1" }
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _movieRepositoryMock
                .Setup(x => x.GetMovieByIdAsync(movieId))
                .ReturnsAsync((Movie)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _movieRepositoryMock.Verify(x => x.GetMovieByIdAsync(movieId), Times.Once);
            _mapperMock.Verify(x => x.Map<Movie>(command), Times.Never);
            _movieRepositoryMock.Verify(x => x.UpdateMovieAsync(It.IsAny<Movie>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryFailure_ReturnsFalse()
        {
            // Arrange
            var movieId = Guid.NewGuid().ToString();
            var existingMovie = new Movie
            {
                Id = Guid.Parse(movieId),
                Title = "Original Title",
                Description = "Original Description",
                DurationInMinutes = 100,
                Director = "Original Director",
                ReleaseDate = DateTime.UtcNow.AddDays(-30),
                CertificateRating = CertificateRating.U,
                ViewerRating = 3.5,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-60)
            };

            var command = new UpdateMovieCommand
            {
                Id = movieId,
                Title = "Updated Title",
                Description = "Updated Description",
                DurationInMinutes = 120,
                Director = "Updated Director",
                ReleaseDate = DateTime.UtcNow,
                CertificateRating = CertificateRating.UA,
                ViewerRating = 4.5,
                GenreIds = new List<string> { "genre1" },
                LanguageIds = new List<string> { "lang1" }
            };

            var updatedMovie = new Movie
            {
                Id = Guid.Parse(movieId),
                Title = command.Title,
                Description = command.Description,
                DurationInMinutes = command.DurationInMinutes,
                Director = command.Director,
                ReleaseDate = command.ReleaseDate,
                CertificateRating = command.CertificateRating,
                ViewerRating = command.ViewerRating,
                IsActive = existingMovie.IsActive,
                CreatedAt = existingMovie.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            _validatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _movieRepositoryMock
                .Setup(x => x.GetMovieByIdAsync(movieId))
                .ReturnsAsync(existingMovie);

            _mapperMock
                .Setup(x => x.Map<Movie>(command))
                .Returns(updatedMovie);

            _movieRepositoryMock
                .Setup(x => x.UpdateMovieAsync(updatedMovie, command.GenreIds, command.LanguageIds))
                .ReturnsAsync(false); // Repository returns false

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _movieRepositoryMock.Verify(x => x.GetMovieByIdAsync(movieId), Times.Once);
            _mapperMock.Verify(x => x.Map<Movie>(command), Times.Once);
            _movieRepositoryMock.Verify(x => x.UpdateMovieAsync(updatedMovie, command.GenreIds, command.LanguageIds), Times.Once);
        }
    }
} 