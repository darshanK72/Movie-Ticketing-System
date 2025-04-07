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
    public class ResetPasswordCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IValidator<ResetPasswordCommand>> _validatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ResetPasswordCommandHandler _handler;

        public ResetPasswordCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<ResetPasswordCommand>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new ResetPasswordCommandHandler(_userRepositoryMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            var command = new ResetPasswordCommand
            {
                Email = "test@example.com",
                Token = "valid-token",
                NewPassword = "NewPassword123!"
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userRepositoryMock.Setup(x => x.ResetPassword(command.Email, command.Token, command.NewPassword))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.ResetPassword(
                command.Email,
                command.Token,
                command.NewPassword), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new ResetPasswordCommand
            {
                Email = "invalid-email",
                Token = "",
                NewPassword = "weak"
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "Invalid email format"));

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.ResetPassword(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidToken_ReturnsFalse()
        {
            // Arrange
            var command = new ResetPasswordCommand
            {
                Email = "test@example.com",
                Token = "invalid-token",
                NewPassword = "NewPassword123!"
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userRepositoryMock.Setup(x => x.ResetPassword(command.Email, command.Token, command.NewPassword))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.ResetPassword(
                command.Email,
                command.Token,
                command.NewPassword), Times.Once);
        }
    }
} 