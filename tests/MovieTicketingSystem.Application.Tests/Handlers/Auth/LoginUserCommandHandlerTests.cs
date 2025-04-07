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
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using Xunit;

namespace MovieTicketingSystem.Application.Tests.Handlers.Auth
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IValidator<LoginUserCommand>> _validatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<LoginUserCommand>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new LoginUserCommandHandler(_userRepositoryMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsTokenResponse()
        {
            // Arrange
            var command = new LoginUserCommand
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var expectedTokenResponse = new TokenResponse
            {
                AccessToken = "access-token",
                RefreshToken = "refresh-token",
                ExpiresIn = 3600
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userRepositoryMock.Setup(x => x.LoginUser(command.Email, command.Password))
                .ReturnsAsync(expectedTokenResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedTokenResponse);
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.LoginUser(command.Email, command.Password), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new LoginUserCommand
            {
                Email = "invalid-email",
                Password = ""
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "Invalid email format"));

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.LoginUser(
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var command = new LoginUserCommand
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _userRepositoryMock.Setup(x => x.LoginUser(command.Email, command.Password))
                .ReturnsAsync((TokenResponse)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.LoginUser(command.Email, command.Password), Times.Once);
        }
    }
} 