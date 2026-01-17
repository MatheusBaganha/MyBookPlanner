using System.Net;
using System.Linq.Expressions;
using Moq;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Service.Services;
using MyBookPlanner.Tests.Fixtures;
using Bogus;
using MyBookPlanner.Domain.Constantes;

namespace MyBookPlanner.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserFixture _userFixture;
        private readonly Mock<IGenericRepository> _genericRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly UserService _service;
        private readonly Faker _faker;

        public UserServiceTests()
        {
            _userFixture = new UserFixture();
            _genericRepoMock = new Mock<IGenericRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _service = new UserService(_genericRepoMock.Object, _userRepoMock.Object);
            _faker = new Faker();
        }

        #region GetUserById

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = _userFixture.CreateValidUser();
            _genericRepoMock
                .Setup(r => r.GetFirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            // Act
            var result = await _service.GetUserById(user.Id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(user.Id, result.Data.Id);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNotFoundMessage_WhenUserDoesNotExist()
        {
            // Arrange
            _genericRepoMock
                .Setup(r => r.GetFirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _service.GetUserById(999);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, result.Erros);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            _genericRepoMock
                .Setup(r => r.GetFirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .ThrowsAsync(new Exception("Database down"));

            // Act
            var result = await _service.GetUserById(1);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("Database down", result.Erros[0]);
        }

        #endregion

        #region UpdateUser

        [Fact]
        public async Task UpdateUser_ShouldReturnUpdatedUser_WhenValid()
        {
            // Arrange
            var user = _userFixture.CreateValidUser();
            var dto = new UpdateUserDTO
            {
                Username = _faker.Internet.UserName(),
                Email = _faker.Internet.Email(),
                Biography = _faker.Lorem.Paragraph()
            };

            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);
            _userRepoMock.Setup(r => r.DoesUserExists(dto.Email))
                .ReturnsAsync((User)null);
            _genericRepoMock.Setup(r => r.UpdateAsync<User>(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateUser(user.Id, dto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(dto.Username, result.Data.Username);
            Assert.Equal(dto.Email, result.Data.Email);
            Assert.Equal(dto.Biography, result.Data.Biography);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var dto = new UpdateUserDTO
            {
                Username = _faker.Internet.UserName(),
                Email = _faker.Internet.Email(),
                Biography = _faker.Lorem.Paragraph()
            };
            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _service.UpdateUser(999, dto);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, result.Erros);

        }

        [Fact]
        public async Task UpdateUser_ShouldReturnConflict_WhenEmailAlreadyInUse()
        {
            // Arrange
            var user = _userFixture.CreateValidUser();
            var anotherUser = _userFixture.CreateValidUser();
            var dto = new UpdateUserDTO
            {
                Username = _faker.Internet.UserName(),
                Email = _faker.Internet.Email(),
                Biography = _faker.Lorem.Paragraph()
            };

            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);
            _userRepoMock.Setup(r => r.DoesUserExists(dto.Email))
                .ReturnsAsync(anotherUser);

            // Act
            var result = await _service.UpdateUser(user.Id, dto);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Contains(ErrorMessages.UserEmailAlreadyExists, result.Erros);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            var dto = new UpdateUserDTO
            {
                Username = _faker.Internet.UserName(),
                Email = _faker.Internet.Email(),
                Biography = _faker.Lorem.Paragraph()
            };

            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .ThrowsAsync(new Exception("Database down"));

            // Act
            var result = await _service.UpdateUser(1, dto);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("Database down", result.Erros[0]);
        }

        #endregion

        #region DeleteUser

        [Fact]
        public async Task DeleteUser_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = _userFixture.CreateValidUser();
            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);
            _genericRepoMock.Setup(r => r.DeleteAsync<User>(user))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteUser(user.Id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(user.Id, result.Data.Id);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFoundMessage_WhenUserDoesNotExist()
        {
            // Arrange
            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _service.DeleteUser(999);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, result.Erros);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>()))
                .ThrowsAsync(new Exception("Database down"));

            // Act
            var result = await _service.DeleteUser(1);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("Database down", result.Erros[0]);
        }

        #endregion
    }
}
