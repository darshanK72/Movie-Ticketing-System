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
using Xunit;

namespace MovieTicketingSystem.Application.Tests.Handlers.Auth
{
    public class LogoutUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IValidator<LogoutUserCommand>> _validatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly LogoutUserCommandHandler _handler;

        public LogoutUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<LogoutUserCommand>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new LogoutUserCommandHandler(_userRepositoryMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            var command = new LogoutUserCommand
            {
                Email = "test@example.com"
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userRepositoryMock.Setup(x => x.LogoutUser(command.Email))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.LogoutUser(command.Email), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new LogoutUserCommand
            {
                Email = "invalid-email"
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "Invalid email format"));

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.LogoutUser(
                It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryFailure_ReturnsFalse()
        {
            // Arrange
            var command = new LogoutUserCommand
            {
                Email = "test@example.com"
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userRepositoryMock.Setup(x => x.LogoutUser(command.Email))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.LogoutUser(command.Email), Times.Once);
        }
    }
} 