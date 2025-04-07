using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Infrastructure.Repositories;
using Xunit;

namespace MovieTicketingSystem.Infrastructure.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            // Setup UserManager mock
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                userStoreMock.Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            // Setup RoleManager mock
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object, null!, null!, null!, null!);

            // Setup SignInManager mock
            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null!, null!, null!, null!);

            // Setup Configuration mock
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x["Jwt:Key"]).Returns("YourSecretKeyHere12345678901234567890");
            _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("MovieTicketingSystem");
            _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("MovieTicketingSystemUsers");
            _configurationMock.Setup(x => x["Jwt:ExpireDays"]).Returns("7");

            _repository = new UserRepository(
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _signInManagerMock.Object,
                _configurationMock.Object);
        }

        [Fact]
        public async Task RegisterUser_Success_ReturnsTrue()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "test@example.com",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                Password = "Password123!",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.CreateAsync(user, user.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.AddClaimAsync(user, It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            _roleManagerMock.Setup(x => x.RoleExistsAsync(user.Role.ToString()))
                .ReturnsAsync(false);

            _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.AddToRoleAsync(user, user.Role.ToString()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.RegisterUser(user);

            // Assert
            result.Should().BeTrue();
            _userManagerMock.Verify(x => x.CreateAsync(user, user.Password), Times.Once);
            _userManagerMock.Verify(x => x.AddClaimAsync(user, It.Is<Claim>(c => c.Type == "FirstName")), Times.Once);
            _userManagerMock.Verify(x => x.AddClaimAsync(user, It.Is<Claim>(c => c.Type == "LastName")), Times.Once);
            _roleManagerMock.Verify(x => x.RoleExistsAsync(user.Role.ToString()), Times.Once);
            _roleManagerMock.Verify(x => x.CreateAsync(It.Is<IdentityRole>(r => r.Name == user.Role.ToString())), Times.Once);
            _userManagerMock.Verify(x => x.AddToRoleAsync(user, user.Role.ToString()), Times.Once);
        }

        [Fact]
        public async Task RegisterUser_Failure_ReturnsFalse()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "test@example.com",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                Password = "Password123!",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.CreateAsync(user, user.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User already exists" }));

            // Act
            var result = await _repository.RegisterUser(user);

            // Assert
            result.Should().BeFalse();
            _userManagerMock.Verify(x => x.CreateAsync(user, user.Password), Times.Once);
            _userManagerMock.Verify(x => x.AddClaimAsync(user, It.IsAny<Claim>()), Times.Never);
            _roleManagerMock.Verify(x => x.RoleExistsAsync(It.IsAny<string>()), Times.Never);
            _roleManagerMock.Verify(x => x.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
            _userManagerMock.Verify(x => x.AddToRoleAsync(user, It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task LoginUser_Success_ReturnsTokenResponse()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(email, password, false, false))
                .ReturnsAsync(SignInResult.Success);

            _userManagerMock.Setup(x => x.UpdateSecurityStampAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { user.Role.ToString() });

            _userManagerMock.Setup(x => x.GenerateUserTokenAsync(user, "Default", "RefreshToken"))
                .ReturnsAsync("refresh-token");

            // Act
            var result = await _repository.LoginUser(email, password);

            // Assert
            result.Should().NotBeNull();
            result!.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().Be("refresh-token");
            result.ExpiresIn.Should().BeGreaterThan(0);
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _signInManagerMock.Verify(x => x.PasswordSignInAsync(email, password, false, false), Times.Once);
            _userManagerMock.Verify(x => x.UpdateSecurityStampAsync(user), Times.Once);
            _userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Once);
            _userManagerMock.Verify(x => x.GenerateUserTokenAsync(user, "Default", "RefreshToken"), Times.Once);
        }

        [Fact]
        public async Task LoginUser_UserNotFound_ReturnsNull()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var password = "Password123!";

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _repository.LoginUser(email, password);

            // Assert
            result.Should().BeNull();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _signInManagerMock.Verify(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task LoginUser_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var email = "test@example.com";
            var password = "WrongPassword";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(email, password, false, false))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _repository.LoginUser(email, password);

            // Assert
            result.Should().BeNull();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _signInManagerMock.Verify(x => x.PasswordSignInAsync(email, password, false, false), Times.Once);
            _userManagerMock.Verify(x => x.UpdateSecurityStampAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LogoutUser_Success_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.UpdateSecurityStampAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.LogoutUser(email);

            // Assert
            result.Should().BeTrue();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.UpdateSecurityStampAsync(user), Times.Once);
        }

        [Fact]
        public async Task LogoutUser_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var email = "nonexistent@example.com";

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _repository.LogoutUser(email);

            // Assert
            result.Should().BeFalse();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.UpdateSecurityStampAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ForgotPassword_Success_ReturnsToken()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("reset-token");

            // Act
            var result = await _repository.ForgotPassword(email);

            // Assert
            result.Should().Be("reset-token");
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(user), Times.Once);
        }

        [Fact]
        public async Task ForgotPassword_UserNotFound_ReturnsNull()
        {
            // Arrange
            var email = "nonexistent@example.com";

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _repository.ForgotPassword(email);

            // Assert
            result.Should().BeNull();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ResetPassword_Success_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var token = "reset-token";
            var newPassword = "NewPassword123!";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.ResetPasswordAsync(user, token, newPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.ResetPassword(email, token, newPassword);

            // Assert
            result.Should().BeTrue();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.ResetPasswordAsync(user, token, newPassword), Times.Once);
        }

        [Fact]
        public async Task ResetPassword_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var token = "reset-token";
            var newPassword = "NewPassword123!";

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _repository.ResetPassword(email, token, newPassword);

            // Assert
            result.Should().BeFalse();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ResetPassword_InvalidToken_ReturnsFalse()
        {
            // Arrange
            var email = "test@example.com";
            var token = "invalid-token";
            var newPassword = "NewPassword123!";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.ResetPasswordAsync(user, token, newPassword))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid token" }));

            // Act
            var result = await _repository.ResetPassword(email, token, newPassword);

            // Assert
            result.Should().BeFalse();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.ResetPasswordAsync(user, token, newPassword), Times.Once);
        }

        [Fact]
        public async Task RefreshToken_Success_ReturnsNewTokenResponse()
        {
            // Arrange
            var email = "test@example.com";
            var refreshToken = "valid-refresh-token";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.VerifyUserTokenAsync(user, "Default", "RefreshToken", refreshToken))
                .ReturnsAsync(true);

            _userManagerMock.Setup(x => x.UpdateSecurityStampAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { user.Role.ToString() });

            _userManagerMock.Setup(x => x.GenerateUserTokenAsync(user, "Default", "RefreshToken"))
                .ReturnsAsync("new-refresh-token");

            // Act
            var result = await _repository.RefreshToken(refreshToken, email);

            // Assert
            result.Should().NotBeNull();
            result!.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().Be("new-refresh-token");
            result.ExpiresIn.Should().BeGreaterThan(0);
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.VerifyUserTokenAsync(user, "Default", "RefreshToken", refreshToken), Times.Once);
            _userManagerMock.Verify(x => x.UpdateSecurityStampAsync(user), Times.Once);
            _userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Once);
            _userManagerMock.Verify(x => x.GenerateUserTokenAsync(user, "Default", "RefreshToken"), Times.Once);
        }

        [Fact]
        public async Task RefreshToken_UserNotFound_ReturnsNull()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var refreshToken = "valid-refresh-token";

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _repository.RefreshToken(refreshToken, email);

            // Assert
            result.Should().BeNull();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.VerifyUserTokenAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RefreshToken_InvalidToken_ReturnsNull()
        {
            // Arrange
            var email = "test@example.com";
            var refreshToken = "invalid-refresh-token";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.VerifyUserTokenAsync(user, "Default", "RefreshToken", refreshToken))
                .ReturnsAsync(false);

            // Act
            var result = await _repository.RefreshToken(refreshToken, email);

            // Assert
            result.Should().BeNull();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.VerifyUserTokenAsync(user, "Default", "RefreshToken", refreshToken), Times.Once);
            _userManagerMock.Verify(x => x.UpdateSecurityStampAsync(It.IsAny<User>()), Times.Never);
        }
    }
} 