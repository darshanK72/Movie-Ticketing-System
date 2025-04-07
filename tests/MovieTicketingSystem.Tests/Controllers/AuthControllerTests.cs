using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieTicketingSystem.Application.Commands.Auth;
using MovieTicketingSystem.Controllers;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;
using Xunit;

namespace MovieTicketingSystem.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _controller = new AuthController(_mediatorMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Register_Success_ReturnsOkResult()
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

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Register(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Register_Failure_ReturnsBadRequest()
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

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Register(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Login_Success_ReturnsOkResult()
        {
            // Arrange
            var command = new LoginUserCommand
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var tokenResponse = new TokenResponse
            {
                AccessToken = "access-token",
                RefreshToken = "refresh-token",
                ExpiresIn = 3600
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tokenResponse);

            // Act
            var result = await _controller.Login(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Login_Failure_ReturnsUnauthorized()
        {
            // Arrange
            var command = new LoginUserCommand
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync((TokenResponse)null!);

            // Act
            var result = await _controller.Login(command);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Logout_Success_ReturnsOkResult()
        {
            // Arrange
            var command = new LogoutUserCommand
            {
                Email = "test@example.com"
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Login(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Logout_Failure_ReturnsUnauthorized()
        {
            // Arrange
            var command = new LogoutUserCommand
            {
                Email = "test@example.com"
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Login(command);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ForgotPassword_Success_ReturnsOkResult()
        {
            // Arrange
            var command = new ForgetPasswordCommand
            {
                Email = "test@example.com"
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync("reset-token");

            // Act
            var result = await _controller.ForgotPassword(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ForgotPassword_Failure_ReturnsBadRequest()
        {
            // Arrange
            var command = new ForgetPasswordCommand
            {
                Email = "nonexistent@example.com"
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync((string)null!);

            // Act
            var result = await _controller.ForgotPassword(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ResetPassword_Success_ReturnsOkResult()
        {
            // Arrange
            var command = new ResetPasswordCommand
            {
                Email = "test@example.com",
                Token = "reset-token",
                NewPassword = "NewPassword123!"
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.ResetPassword(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ResetPassword_Failure_ReturnsBadRequest()
        {
            // Arrange
            var command = new ResetPasswordCommand
            {
                Email = "test@example.com",
                Token = "invalid-token",
                NewPassword = "NewPassword123!"
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.ResetPassword(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RefreshToken_Success_ReturnsOkResult()
        {
            // Arrange
            var command = new RefreshTokenCommand
            {
                Email = "test@example.com",
                RefreshToken = "valid-refresh-token"
            };

            var tokenResponse = new TokenResponse
            {
                AccessToken = "new-access-token",
                RefreshToken = "new-refresh-token",
                ExpiresIn = 3600
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tokenResponse);

            // Act
            var result = await _controller.RefreshToken(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RefreshToken_Failure_ReturnsUnauthorized()
        {
            // Arrange
            var command = new RefreshTokenCommand
            {
                Email = "test@example.com",
                RefreshToken = "invalid-refresh-token"
            };

            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync((TokenResponse)null!);

            // Act
            var result = await _controller.RefreshToken(command);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult!.Value.Should().NotBeNull();
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
} 