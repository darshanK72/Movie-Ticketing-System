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

namespace MovieTicketingSystem.Application.Tests.Handlers.Movies.Queries
{
    public class GetAllMoviesQueryHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllMoviesQueryHandler _handler;

        public GetAllMoviesQueryHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllMoviesQueryHandler(_movieRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenMoviesExist_ReturnsMovieDTOs()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie
                {
                    Id = Guid.NewGuid(),
                    Title = "Movie 1",
                    Description = "Description 1",
                    DurationInMinutes = 120,
                    Director = "Director 1",
                    ReleaseDate = DateTime.UtcNow,
                    CertificateRating = CertificateRating.UA,
                    ViewerRating = 4.5,
                    IsActive = true
                },
                new Movie
                {
                    Id = Guid.NewGuid(),
                    Title = "Movie 2",
                    Description = "Description 2",
                    DurationInMinutes = 150,
                    Director = "Director 2",
                    ReleaseDate = DateTime.UtcNow,
                    CertificateRating = CertificateRating.A,
                    ViewerRating = 4.0,
                    IsActive = true
                }
            };

            var movieDtos = movies.Select(m => new MovieDTO
            {
                Id = m.Id.ToString(),
                Title = m.Title,
                Description = m.Description,
                DurationInMinutes = m.DurationInMinutes,
                Director = m.Director,
                ReleaseDate = m.ReleaseDate,
                CertificateRating = m.CertificateRating.ToString(),
                ViewerRating = m.ViewerRating
            }).ToList();

            _movieRepositoryMock
                .Setup(x => x.GetAllMoviesAsync())
                .ReturnsAsync(movies);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<MovieDTO>>(movies))
                .Returns(movieDtos);

            var query = new GetAllMoviesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(movieDtos);
            result.Should().HaveCount(2);
            _movieRepositoryMock.Verify(x => x.GetAllMoviesAsync(), Times.Once);
            _mapperMock.Verify(x => x.Map<IEnumerable<MovieDTO>>(movies), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoMoviesExist_ReturnsEmptyList()
        {
            // Arrange
            var movies = new List<Movie>();
            var movieDtos = new List<MovieDTO>();

            _movieRepositoryMock
                .Setup(x => x.GetAllMoviesAsync())
                .ReturnsAsync(movies);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<MovieDTO>>(movies))
                .Returns(movieDtos);

            var query = new GetAllMoviesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _movieRepositoryMock.Verify(x => x.GetAllMoviesAsync(), Times.Once);
            _mapperMock.Verify(x => x.Map<IEnumerable<MovieDTO>>(movies), Times.Once);
        }
    }
} 