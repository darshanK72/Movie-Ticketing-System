using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using MovieTicketingSystem.Application.Commands.Auth;
using MovieTicketingSystem.Application.Handlers.Auth;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;
using Xunit;

namespace MovieTicketingSystem.Application.Tests.Handlers.Auth
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IValidator<RegisterUserCommand>> _validatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<RegisterUserCommand>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = "Password123!",
                Role = "Customer"
            };

            var user = new User
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                UserName = command.Email,
                Password = command.Password
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mapperMock.Setup(x => x.Map<User>(command))
                .Returns(user);

            _userRepositoryMock.Setup(x => x.RegisterUser(user))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<User>(command), Times.Once);
            _userRepositoryMock.Verify(x => x.RegisterUser(user), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                FirstName = "",
                LastName = "",
                Email = "invalid-email",
                Password = "weak",
                Role = ""
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "Invalid email format"));

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<User>(command), Times.Never);
            _userRepositoryMock.Verify(x => x.RegisterUser(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryFailure_ReturnsFalse()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = "Password123!",
                Role = "Customer"
            };

            var user = new User
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                UserName = command.Email,
                Password = command.Password
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mapperMock.Setup(x => x.Map<User>(command))
                .Returns(user);

            _userRepositoryMock.Setup(x => x.RegisterUser(user))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<User>(command), Times.Once);
            _userRepositoryMock.Verify(x => x.RegisterUser(user), Times.Once);
        }
    }
} 