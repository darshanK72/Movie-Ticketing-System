using FluentAssertions;
using Moq;
using MovieTicketingSystem.Application.Commands.Movies;
using MovieTicketingSystem.Application.Handlers.Movies;
using MovieTicketingSystem.Domain.Contracts.Repository;
using Xunit;

namespace MovieTicketingSystem.Application.Tests.Handlers.Movies.Commands
{
    public class DeleteMovieCommandHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly DeleteMovieCommandHandler _handler;

        public DeleteMovieCommandHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _handler = new DeleteMovieCommandHandler(_movieRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidMovieId_DeletesMovie()
        {
            // Arrange
            var movieId = Guid.NewGuid().ToString();
            var command = new DeleteMovieCommand(movieId);

            _movieRepositoryMock
                .Setup(x => x.DeleteMovieAsync(movieId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _movieRepositoryMock.Verify(x => x.DeleteMovieAsync(movieId), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryFailure_ReturnsFalse()
        {
            // Arrange
            var movieId = Guid.NewGuid().ToString();
            var command = new DeleteMovieCommand(movieId);

            _movieRepositoryMock
                .Setup(x => x.DeleteMovieAsync(movieId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _movieRepositoryMock.Verify(x => x.DeleteMovieAsync(movieId), Times.Once);
        }
    }
} 