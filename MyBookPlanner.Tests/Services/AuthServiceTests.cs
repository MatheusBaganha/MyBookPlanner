using System.Net;
using Bogus;
using Moq;
using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Service.Interfaces;
using MyBookPlanner.Service.Services;
using MyBookPlanner.Tests.Fixtures;
using SecureIdentity.Password;

namespace MyBookPlanner.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly UserFixture _userFixture;
        private readonly Mock<IGenericRepository> _genericRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthService _service;
        private readonly Faker _faker;

        public AuthServiceTests()
        {
            _faker = new Faker();
            _userFixture = new UserFixture();
            _genericRepoMock = new Mock<IGenericRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _service = new AuthService(_genericRepoMock.Object, _userRepoMock.Object, _tokenServiceMock.Object);
        }

        #region Login

        [Fact]
        public async Task Login_ShouldReturnUserWithToken_WhenCredentialsAreValid()
        {
            // Arrange
            var password = _faker.Internet.Password();
            var user = _userFixture.CreateValidUser();
            user.PasswordHash = PasswordHasher.Hash(password);

            var dto = new UserLoginDTO { Email = user.Email, Password = password };
            _userRepoMock.Setup(r => r.DoesUserExists(user.Email)).ReturnsAsync(user);
            _tokenServiceMock.Setup(t => t.GenerateToken(user)).Returns("fake_token");

            // Act
            var result = await _service.Login(dto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(user.Email, result.Data.Email);
            Assert.Equal("fake_token", result.Data.UserToken);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserDoesNotExist()
        {
            // Arrange
            var dto = new UserLoginDTO { Email = "nonexistent@email.com", Password = "123" };
            _userRepoMock.Setup(r => r.DoesUserExists(dto.Email)).ReturnsAsync((User)null);

            // Act
            var result = await _service.Login(dto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Contains(ErrorMessages.InvalidUser, result.Erros);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenPasswordIsInvalid()
        {
            // Arrange
            var user = _userFixture.CreateValidUser();
            user.PasswordHash = PasswordHasher.Hash("correct_password");
            var dto = new UserLoginDTO { Email = user.Email, Password = "wrong_password" };
            _userRepoMock.Setup(r => r.DoesUserExists(user.Email)).ReturnsAsync(user);

            // Act
            var result = await _service.Login(dto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Contains(ErrorMessages.InvalidUser, result.Erros);
        }

        [Fact]
        public async Task Login_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            var dto = new UserLoginDTO { Email = "x@x.com", Password = "123" };
            _userRepoMock.Setup(r => r.DoesUserExists(dto.Email)).ThrowsAsync(new Exception("Database down"));

            // Act
            var result = await _service.Login(dto);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("Database down", result.Erros[0]);
        }

        #endregion

        #region Register

        [Fact]
        public async Task Register_ShouldCreateUserAndReturnToken_WhenEmailIsNew()
        {
            // Arrange
            var password = _faker.Internet.Password();
            var dto = new RegisterUserDTO
            {
                Email = _faker.Internet.Email(),
                Username = _faker.Internet.UserName(),
                Password = password
            };

            _userRepoMock.Setup(r => r.DoesUserExists(dto.Email)).ReturnsAsync((User)null);
            _tokenServiceMock.Setup(t => t.GenerateToken(It.IsAny<User>())).Returns("fake_token");

            // Act
            var result = await _service.Register(dto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(dto.Email, result.Data.Email);
            Assert.Equal("fake_token", result.Data.UserToken);
        }

        [Fact]
        public async Task Register_ShouldReturnConflict_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingUser = _userFixture.CreateValidUser();
            var dto = new RegisterUserDTO
            {
                Email = existingUser.Email,
                Username = _faker.Internet.UserName(),
                Password = _faker.Internet.Password()
            };

            _userRepoMock.Setup(r => r.DoesUserExists(dto.Email)).ReturnsAsync(existingUser);

            // Act
            var result = await _service.Register(dto);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Contains(ErrorMessages.UserEmailAlreadyExists, result.Erros);
        }

        [Fact]
        public async Task Register_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            var dto = new RegisterUserDTO
            {
                Email = _faker.Internet.Email(),
                Username = _faker.Internet.UserName(),
                Password = _faker.Internet.Password()
            };
            _userRepoMock.Setup(r => r.DoesUserExists(dto.Email)).ThrowsAsync(new Exception("Database down"));

            // Act
            var result = await _service.Register(dto);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("Database down", result.Erros[0]);
        }

        #endregion
    }
}
