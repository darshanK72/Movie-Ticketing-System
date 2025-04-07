using AutoMapper;
using FluentAssertions;
using Moq;
using MovieTicketingSystem.Application.Handlers.Movies;
using MovieTicketingSystem.Application.Queries.Movies;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;
using Xunit;

namespace MovieTicketingSystem.Application.Tests.Handlers.Movies
{
    public class GetMovieByIdQueryHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetMovieByIdQueryHandler _handler;

        public GetMovieByIdQueryHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetMovieByIdQueryHandler(_movieRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidMovieId_ReturnsMovieDTO()
        {
            // Arrange
            var movieId = Guid.NewGuid();
            var movie = new Movie
            {
                Id = movieId,
                Title = "Test Movie",
                Description = "Test Description",
                DurationInMinutes = 120,
                Director = "Test Director",
                ReleaseDate = DateTime.UtcNow,
                CertificateRating = CertificateRating.UA,
                ViewerRating = 4.5,
                IsActive = true
            };

            var movieDto = new MovieDTO
            {
                Id = movieId.ToString(),
                Title = "Test Movie",
                Description = "Test Description",
                DurationInMinutes = 120,
                Director = "Test Director",
                ReleaseDate = movie.ReleaseDate,
                CertificateRating = CertificateRating.UA.ToString(),
                ViewerRating = 4.5
            };

            _movieRepositoryMock
                .Setup(x => x.GetMovieByIdAsync(movieId.ToString()))
                .ReturnsAsync(movie);

            _mapperMock
                .Setup(x => x.Map<MovieDTO>(movie))
                .Returns(movieDto);

            var query = new GetMovieByIdQuery(movieId.ToString());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(movieDto);
            _movieRepositoryMock.Verify(x => x.GetMovieByIdAsync(movieId.ToString()), Times.Once);
            _mapperMock.Verify(x => x.Map<MovieDTO>(movie), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidMovieId_ReturnsNull()
        {
            // Arrange
            var movieId = "invalid-id";
            _movieRepositoryMock
                .Setup(x => x.GetMovieByIdAsync(movieId))
                .ReturnsAsync((Movie)null!);

            // Setup mapper to return null when given null
            _mapperMock
                .Setup(x => x.Map<MovieDTO>(null!))
                .Returns((MovieDTO)null!);

            var query = new GetMovieByIdQuery(movieId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _movieRepositoryMock.Verify(x => x.GetMovieByIdAsync(movieId), Times.Once);
            _mapperMock.Verify(x => x.Map<MovieDTO>(null!), Times.Once);
        }
    }
} 