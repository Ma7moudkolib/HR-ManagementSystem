using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Service;
using Service.Contracts;
using Shared.DataTransferObjects;
using Xunit;

namespace CompanyEmployees.Tests
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<ILoggerManager> _mockLoggerManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _mockLoggerManager = new Mock<ILoggerManager>();
            _mockMapper = new Mock<IMapper>();
            _mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);
            _mockConfiguration = new Mock<IConfiguration>();

            _authenticationService = new AuthenticationService(
                _mockLoggerManager.Object,
                _mockMapper.Object,
                _mockUserManager.Object,
                _mockConfiguration.Object);
        }

        #region RegisterUser Tests

        [Fact]
        public async Task RegisterUser_WithValidData_ReturnsSuccessIdentityResult()
        {
            // Arrange
            var userForRegistration = new UserForRegistrationDto
            {
                UserName = "john.doe",
                Email = "john@example.com",
                Password = "SecurePassword123!",
                Roles = new[] { "User" }
            };
            var user = new User { UserName = "john.doe", Email = "john@example.com" };

            _mockMapper.Setup(x => x.Map<User>(userForRegistration)).Returns(user);
            _mockUserManager.Setup(x => x.CreateAsync(user, userForRegistration.Password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.AddToRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authenticationService.RegisterUser(userForRegistration);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            _mockUserManager.Verify(x => x.CreateAsync(user, userForRegistration.Password), Times.Once);
            _mockUserManager.Verify(x => x.AddToRolesAsync(user, It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Fact]
        public async Task RegisterUser_WithDuplicateUsername_ReturnsFailedIdentityResult()
        {
            // Arrange
            var userForRegistration = new UserForRegistrationDto
            {
                UserName = "john.doe",
                Email = "john@example.com",
                Password = "SecurePassword123!",
                Roles = new[] { "User" }
            };
            var user = new User { UserName = "john.doe" };
            var identityResult = IdentityResult.Failed(
                new IdentityError { Code = "DuplicateUserName", Description = "User name is already taken" });

            _mockMapper.Setup(x => x.Map<User>(userForRegistration)).Returns(user);
            _mockUserManager.Setup(x => x.CreateAsync(user, userForRegistration.Password)).ReturnsAsync(identityResult);

            // Act
            var result = await _authenticationService.RegisterUser(userForRegistration);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            _mockUserManager.Verify(x => x.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUser_WithMultipleRoles_AddsAllRolesToUser()
        {
            // Arrange
            var roles = new[] { "Admin", "Manager" };
            var userForRegistration = new UserForRegistrationDto
            {
                UserName = "john.doe",
                Email = "john@example.com",
                Password = "SecurePassword123!",
                Roles = roles
            };
            var user = new User { UserName = "john.doe" };

            _mockMapper.Setup(x => x.Map<User>(userForRegistration)).Returns(user);
            _mockUserManager.Setup(x => x.CreateAsync(user, userForRegistration.Password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.AddToRolesAsync(user, roles)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authenticationService.RegisterUser(userForRegistration);

            // Assert
            Assert.True(result.Succeeded);
            _mockUserManager.Verify(x => x.AddToRolesAsync(user, roles), Times.Once);
        }

        [Fact]
        public async Task RegisterUser_WhenCreationFails_DoesNotAddRoles()
        {
            // Arrange
            var userForRegistration = new UserForRegistrationDto
            {
                UserName = "john.doe",
                Email = "john@example.com",
                Password = "WeakPassword",
                Roles = new[] { "User" }
            };
            var user = new User { UserName = "john.doe" };
            var identityResult = IdentityResult.Failed(
                new IdentityError { Code = "PasswordTooShort", Description = "Password is too short" });

            _mockMapper.Setup(x => x.Map<User>(userForRegistration)).Returns(user);
            _mockUserManager.Setup(x => x.CreateAsync(user, userForRegistration.Password)).ReturnsAsync(identityResult);

            // Act
            var result = await _authenticationService.RegisterUser(userForRegistration);

            // Assert
            Assert.False(result.Succeeded);
            _mockUserManager.Verify(x => x.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        #endregion

        #region ValidateUser Tests

        [Fact]
        public async Task ValidateUser_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            var userForAuth = new UserForAuthDto { UserName = "john.doe", Password = "SecurePassword123!" };
            var user = new User { Id = "1", UserName = "john.doe", Email = "john@example.com" };

            _mockUserManager.Setup(x => x.FindByNameAsync("john.doe")).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, "SecurePassword123!")).ReturnsAsync(true);

            // Act
            var result = await _authenticationService.ValidateUser(userForAuth);

            // Assert
            Assert.True(result);
            _mockUserManager.Verify(x => x.FindByNameAsync("john.doe"), Times.Once);
            _mockUserManager.Verify(x => x.CheckPasswordAsync(user, "SecurePassword123!"), Times.Once);
        }

        [Fact]
        public async Task ValidateUser_WithNonExistentUser_ReturnsFalse()
        {
            // Arrange
            var userForAuth = new UserForAuthDto { UserName = "nonexistent.user", Password = "SomePassword123!" };

            _mockUserManager.Setup(x => x.FindByNameAsync("nonexistent.user")).ReturnsAsync((User)null);

            // Act
            var result = await _authenticationService.ValidateUser(userForAuth);

            // Assert
            Assert.False(result);
            _mockLoggerManager.Verify(x => x.LogWarn(It.Is<string>(msg => msg.Contains("nonexistent.user"))), Times.Once);
        }

        [Fact]
        public async Task ValidateUser_WithInvalidPassword_ReturnsFalse()
        {
            // Arrange
            var userForAuth = new UserForAuthDto { UserName = "john.doe", Password = "WrongPassword" };
            var user = new User { Id = "1", UserName = "john.doe" };

            _mockUserManager.Setup(x => x.FindByNameAsync("john.doe")).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, "WrongPassword")).ReturnsAsync(false);

            // Act
            var result = await _authenticationService.ValidateUser(userForAuth);

            // Assert
            Assert.False(result);
            _mockLoggerManager.Verify(x => x.LogWarn(It.Is<string>(msg => msg.Contains("john.doe"))), Times.Once);
        }

        [Fact]
        public async Task ValidateUser_WithValidCredentials_LogsNothing()
        {
            // Arrange
            var userForAuth = new UserForAuthDto { UserName = "john.doe", Password = "SecurePassword123!" };
            var user = new User { Id = "1", UserName = "john.doe" };

            _mockUserManager.Setup(x => x.FindByNameAsync("john.doe")).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, "SecurePassword123!")).ReturnsAsync(true);

            // Act
            var result = await _authenticationService.ValidateUser(userForAuth);

            // Assert
            Assert.True(result);
            _mockLoggerManager.Verify(x => x.LogWarn(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region CreateToken Tests

        // [Fact]
        // public async Task CreateToken_WithValidConfiguration_ReturnsJwtToken()
        // {
        //     // Arrange
        //     var user = new User { Id = "1", UserName = "john.doe" };
        //     var configurationSection = new Mock<IConfigurationSection>();

        //     _mockConfiguration.Setup(x => x["JwtSettings:SecretKey"])
        //         .Returns("ThisIsAVeryLongSecretKeyForAuthenticationPurposesWithAtLeast32Characters");
        //     _mockConfiguration.Setup(x => x.GetSection("JwtSettings")).Returns(configurationSection.Object);
        //     configurationSection.Setup(x => x["validIssuer"]).Returns("YourIssuer");
        //     configurationSection.Setup(x => x["validAudience"]).Returns("YourAudience");
        //     configurationSection.Setup(x => x["expires"]).Returns("60");
        //     _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "Admin" });

        //     // Act
        //     var token = await _authenticationService.CreateToken();

        //     // Assert
        //     Assert.NotNull(token);
        //     Assert.NotEmpty(token);
        //     Assert.IsType<string>(token);
        // }

        // [Fact]
        // public async Task CreateToken_WithMultipleRoles_IncludesAllRolesInToken()
        // {
        //     // Arrange
        //     var user = new User { Id = "1", UserName = "john.doe" };
        //     var roles = new List<string> { "Admin", "Manager" };
        //     var configurationSection = new Mock<IConfigurationSection>();

        //     _mockConfiguration.Setup(x => x["JwtSettings:SecretKey"])
        //         .Returns("ThisIsAVeryLongSecretKeyForAuthenticationPurposesWithAtLeast32Characters");
        //     _mockConfiguration.Setup(x => x.GetSection("JwtSettings")).Returns(configurationSection.Object);
        //     configurationSection.Setup(x => x["validIssuer"]).Returns("YourIssuer");
        //     configurationSection.Setup(x => x["validAudience"]).Returns("YourAudience");
        //     configurationSection.Setup(x => x["expires"]).Returns("60");
        //     _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);

        //     // Act
        //     var token = await _authenticationService.CreateToken();

        //     // Assert
        //     Assert.NotNull(token);
        //     Assert.NotEmpty(token);
        //     Assert.IsType<string>(token);
        // }

        #endregion
    }
}
