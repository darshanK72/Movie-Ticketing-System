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
    public class ForgetPasswordCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IValidator<ForgetPasswordCommand>> _validatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ForgetPasswordCommandHandler _handler;

        public ForgetPasswordCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<ForgetPasswordCommand>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new ForgetPasswordCommandHandler(_userRepositoryMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsResetToken()
        {
            // Arrange
            var command = new ForgetPasswordCommand
            {
                Email = "test@example.com"
            };

            var expectedToken = "reset-token";

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userRepositoryMock.Setup(x => x.ForgotPassword(command.Email))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(expectedToken);
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.ForgotPassword(command.Email), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new ForgetPasswordCommand
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
            _userRepositoryMock.Verify(x => x.ForgotPassword(
                It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_NonExistentUser_ReturnsNull()
        {
            // Arrange
            var command = new ForgetPasswordCommand
            {
                Email = "nonexistent@example.com"
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userRepositoryMock.Setup(x => x.ForgotPassword(command.Email))
                .ReturnsAsync((string)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.ForgotPassword(command.Email), Times.Once);
        }
    }
} 