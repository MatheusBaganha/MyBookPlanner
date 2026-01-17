using System.Net;
using Bogus;
using Moq;
using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Service.Services;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.Tests.Services
{
    public class UserBooksStatisticsServiceTests
    {
        private readonly Mock<IUserBooksRepository> _userBooksRepoMock;
        private readonly UserBooksStatisticsService _service;
        private readonly Faker _faker;

        public UserBooksStatisticsServiceTests()
        {
            _userBooksRepoMock = new Mock<IUserBooksRepository>();
            _service = new UserBooksStatisticsService(_userBooksRepoMock.Object);
            _faker = new Faker();
        }

        #region GetUserBestBook

        [Fact]
        public async Task GetUserBestBook_ShouldReturnBook_WhenExists()
        {
            // Arrange
            var bestBook = new UserBestBookViewModel
            {
                IdBook = 1,
                IdUser = 1,
                Title = _faker.Lorem.Sentence(),
                Author = _faker.Person.FullName,
                ImageUrl = _faker.Internet.Avatar(),
                ReleaseYear = 2020,
                UserScore = 9
            };
            _userBooksRepoMock.Setup(r => r.GetUserBestBook(1)).ReturnsAsync(bestBook);

            // Act
            var result = await _service.GetUserBestBook(1);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(bestBook.IdBook, result.Data.IdBook);
            Assert.Equal(bestBook.Title, result.Data.Title);
            Assert.Equal(bestBook.UserScore, result.Data.UserScore);
        }

        [Fact]
        public async Task GetUserBestBook_ShouldReturnGenericBook_WhenNull()
        {
            // Arrange
            _userBooksRepoMock.Setup(r => r.GetUserBestBook(1)).ReturnsAsync((UserBestBookViewModel)null);

            // Act
            var result = await _service.GetUserBestBook(1);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.IdUser);
            Assert.Equal(0, result.Data.IdBook);
            Assert.Equal(10, result.Data.UserScore);
            Assert.Equal(DateTime.UtcNow.Year, result.Data.ReleaseYear);
            Assert.Equal("MyBookPlannerBook", result.Data.Title);
        }

        [Fact]
        public async Task GetUserBestBook_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            _userBooksRepoMock.Setup(r => r.GetUserBestBook(1)).ThrowsAsync(new Exception("Database down"));

            // Act
            var result = await _service.GetUserBestBook(1);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("Database down", result.Erros[0]);
        }

        #endregion

        #region GetUserStatistics

        [Fact]
        public async Task GetUserStatistics_ShouldReturnCalculatedStats_WhenBooksExist()
        {
            // Arrange
            var userBooks = new List<UserBook>
            {
                new UserBook { ReadingStatus = ReadingStatus.Reading },
                new UserBook { ReadingStatus = ReadingStatus.Read },
                new UserBook { ReadingStatus = ReadingStatus.WishToRead },
                new UserBook { ReadingStatus = ReadingStatus.Read },
                new UserBook { ReadingStatus = ReadingStatus.Reading }
            };
            _userBooksRepoMock.Setup(r => r.GetUserBooks(1)).ReturnsAsync(userBooks);

            // Act
            var result = await _service.GetUserStatistics(1);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(2, result.Data.reading);
            Assert.Equal(2, result.Data.readed);
            Assert.Equal(1, result.Data.wishToRead);
        }

        [Fact]
        public async Task GetUserStatistics_ShouldReturnEmptyStats_WhenNoBooks()
        {
            // Arrange
            _userBooksRepoMock.Setup(r => r.GetUserBooks(1)).ReturnsAsync(new List<UserBook>());

            // Act
            var result = await _service.GetUserStatistics(1);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(0, result.Data.reading);
            Assert.Equal(0, result.Data.readed);
            Assert.Equal(0, result.Data.wishToRead);
        }

        [Fact]
        public async Task GetUserStatistics_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            _userBooksRepoMock.Setup(r => r.GetUserBooks(1)).ThrowsAsync(new Exception("Database down"));

            // Act
            var result = await _service.GetUserStatistics(1);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("Database down", result.Erros[0]);
        }

        #endregion
    }
}
