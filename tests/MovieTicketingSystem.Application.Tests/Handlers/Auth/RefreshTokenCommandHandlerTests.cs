using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using MovieTicketingSystem.Application.Commands.Auth;
using MovieTicketingSystem.Application.Handlers.Auth;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;
using Xunit;

namespace MovieTicketingSystem.Application.Tests.Handlers.Auth
{
    public class RefreshTokenCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IValidator<RefreshTokenCommand>> _validatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RefreshTokenCommandHandler _handler;

        public RefreshTokenCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<RefreshTokenCommand>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new RefreshTokenCommandHandler(_userRepositoryMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTokenResponse()
        {
            // Arrange
            var command = new RefreshTokenCommand
            {
                Email = "test@example.com",
                RefreshToken = "valid-refresh-token"
            };

            var expectedTokenResponse = new TokenResponse
            {
                AccessToken = "new-access-token",
                RefreshToken = "new-refresh-token",
                ExpiresIn = 3600
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userRepositoryMock.Setup(x => x.RefreshToken(command.RefreshToken, command.Email))
                .ReturnsAsync(expectedTokenResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedTokenResponse);
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.RefreshToken(
                command.RefreshToken,
                command.Email), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new RefreshTokenCommand
            {
                Email = "invalid-email",
                RefreshToken = ""
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "Invalid email format"));

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.RefreshToken(
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidToken_ReturnsNull()
        {
            // Arrange
            var command = new RefreshTokenCommand
            {
                Email = "test@example.com",
                RefreshToken = "invalid-refresh-token"
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userRepositoryMock.Setup(x => x.RefreshToken(command.RefreshToken, command.Email))
                .ReturnsAsync((TokenResponse)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.RefreshToken(
                command.RefreshToken,
                command.Email), Times.Once);
        }
    }
} 