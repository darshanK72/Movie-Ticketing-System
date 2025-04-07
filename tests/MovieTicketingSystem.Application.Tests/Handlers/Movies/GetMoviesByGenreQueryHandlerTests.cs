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
    public class GetMoviesByGenreQueryHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetMoviesByGenreQueryHandler _handler;

        public GetMoviesByGenreQueryHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetMoviesByGenreQueryHandler(_movieRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidGenreId_ReturnsMovies()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            var query = new GetMoviesByGenreQuery(genreId.ToString());

            var movies = new List<Movie>
            {
                new Movie
                {
                    Id = Guid.NewGuid(),
                    Title = "Movie 1",
                    Description = "Description 1",
                    DurationInMinutes = 120,
                    Director = "Director 1",
                    Genres = new List<Genre> { new Genre { Id = genreId, Name = "Action" } },
                    ReleaseDate = DateTime.UtcNow,
                    CertificateRating = CertificateRating.UA,
                    ViewerRating = 4.5,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Movie
                {
                    Id = Guid.NewGuid(),
                    Title = "Movie 2",
                    Description = "Description 2",
                    DurationInMinutes = 150,
                    Director = "Director 2",
                    Genres = new List<Genre> { new Genre { Id = genreId, Name = "Action" } },
                    ReleaseDate = DateTime.UtcNow.AddDays(7),
                    CertificateRating = CertificateRating.A,
                    ViewerRating = 4.0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
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
                .Setup(x => x.GetMoviesByGenreAsync(genreId.ToString()))
                .ReturnsAsync(movies);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<MovieDTO>>(movies))
                .Returns(movieDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(movieDtos);
            _movieRepositoryMock.Verify(x => x.GetMoviesByGenreAsync(genreId.ToString()), Times.Once);
            _mapperMock.Verify(x => x.Map<IEnumerable<MovieDTO>>(movies), Times.Once);
        }

        [Fact]
        public async Task Handle_NoMoviesFound_ReturnsEmptyList()
        {
            // Arrange
            var genreId = "nonexistent-genre";
            var query = new GetMoviesByGenreQuery(genreId);

            _movieRepositoryMock
                .Setup(x => x.GetMoviesByGenreAsync(genreId))
                .ReturnsAsync(new List<Movie>());

            _mapperMock
                .Setup(x => x.Map<IEnumerable<MovieDTO>>(new List<Movie>()))
                .Returns(new List<MovieDTO>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _movieRepositoryMock.Verify(x => x.GetMoviesByGenreAsync(genreId), Times.Once);
            _mapperMock.Verify(x => x.Map<IEnumerable<MovieDTO>>(new List<Movie>()), Times.Once);
        }
    }
} 