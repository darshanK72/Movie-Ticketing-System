using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using MovieTicketingSystem.Domain.Contracts.Repository;
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
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                userStoreMock.Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object, null!, null!, null!, null!);

            var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<User>>();
            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                contextAccessorMock.Object,
                claimsFactoryMock.Object,
                null!, null!, null!, null!);

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x["Jwt:Key"]).Returns("your-secret-key-here");
            _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("your-issuer");
            _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("your-audience");
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
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                UserName = "test@example.com",
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
            _roleManagerMock.Verify(x => x.CreateAsync(It.Is<IdentityRole>(r => r.Name == user.Role.ToString())), Times.Once);
            _userManagerMock.Verify(x => x.AddToRoleAsync(user, user.Role.ToString()), Times.Once);
        }

        [Fact]
        public async Task RegisterUser_Failure_ReturnsFalse()
        {
            // Arrange
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                UserName = "test@example.com",
                Password = "Password123!",
                Role = UserRole.User
            };

            _userManagerMock.Setup(x => x.CreateAsync(user, user.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            // Act
            var result = await _repository.RegisterUser(user);

            // Assert
            result.Should().BeFalse();
            _userManagerMock.Verify(x => x.CreateAsync(user, user.Password), Times.Once);
            _userManagerMock.Verify(x => x.AddClaimAsync(user, It.IsAny<Claim>()), Times.Never);
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
                Id = "1",
                FirstName = "Test",
                LastName = "User",
                Email = email,
                UserName = email
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(email, password, false, false))
                .ReturnsAsync(SignInResult.Success);

            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            _userManagerMock.Setup(x => x.GenerateUserTokenAsync(user, "Default", "RefreshToken"))
                .ReturnsAsync("refresh-token");

            // Act
            var result = await _repository.LoginUser(email, password);

            // Assert
            result.Should().NotBeNull();
            result!.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();    
            result.ExpiresIn.Should().BeGreaterThan(0);
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _signInManagerMock.Verify(x => x.PasswordSignInAsync(email, password, false, false), Times.Once);
            _userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Once);
            _userManagerMock.Verify(x => x.GenerateUserTokenAsync(user, "Default", "RefreshToken"), Times.Once);
        }

        [Fact]
        public async Task LoginUser_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrong-password";

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _repository.LoginUser(email, password);

            // Assert
            result.Should().BeNull();
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _signInManagerMock.Verify(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);
        }

        [Fact]
        public async Task LogoutUser_Success_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User { Email = email };

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
            var user = new User { Email = email };
            var expectedToken = "reset-token";

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _repository.ForgotPassword(email);

            // Assert
            result.Should().Be(expectedToken);
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
            var user = new User { Email = email };

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
        public async Task RefreshToken_Success_ReturnsTokenResponse()
        {
            // Arrange
            var email = "test@example.com";
            var refreshToken = "valid-refresh-token";
            var user = new User
            {
                Id = "1",
                FirstName = "Test",
                LastName = "User",
                Email = email,
                UserName = email
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.VerifyUserTokenAsync(user, "Default", "RefreshToken", refreshToken))
                .ReturnsAsync(true);

            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            _userManagerMock.Setup(x => x.GenerateUserTokenAsync(user, "Default", "RefreshToken"))
                .ReturnsAsync("new-refresh-token");

            // Act
            var result = await _repository.RefreshToken(refreshToken, email);

            // Assert
            result.Should().NotBeNull();
            result!.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            result.ExpiresIn.Should().BeGreaterThan(0);
            _userManagerMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
            _userManagerMock.Verify(x => x.VerifyUserTokenAsync(user, "Default", "RefreshToken", refreshToken), Times.Once);
            _userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Once);
            _userManagerMock.Verify(x => x.GenerateUserTokenAsync(user, "Default", "RefreshToken"), Times.Once);
        }

        [Fact]
        public async Task RefreshToken_InvalidToken_ReturnsNull()
        {
            // Arrange
            var email = "test@example.com";
            var refreshToken = "invalid-refresh-token";
            var user = new User { Email = email };

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
            _userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Never);
            _userManagerMock.Verify(x => x.GenerateUserTokenAsync(user, "Default", "RefreshToken"), Times.Never);
        }
    }
} 