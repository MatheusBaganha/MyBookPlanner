using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Service.Services;
using MyBookPlanner.Tests.Collections;
using MyBookPlanner.Tests.Fixtures;
using MyBookPlannerAPI.ViewModels.UserBooks;
using Xunit;

namespace MyBookPlanner.Tests.Services
{
    [Collection(nameof(UserBookCollection))]
    public class UserBooksServiceTests
    {
        private readonly UserBookFixture _fixture;
        private readonly Mock<IGenericRepository> _genericRepoMock;
        private readonly Mock<IUserBooksRepository> _userBooksRepoMock;
        private readonly UserBooksService _service;

        public UserBooksServiceTests(UserBookFixture fixture)
        {
            _fixture = fixture;
            _genericRepoMock = new Mock<IGenericRepository>();
            _userBooksRepoMock = new Mock<IUserBooksRepository>();
            _service = new UserBooksService(_genericRepoMock.Object, _userBooksRepoMock.Object);
        }

        #region GetUserBooksByStatus
        [Fact]
        public async Task GetUserBooksByStatus_ReturnsError_WhenNoBooks()
        {
            // Arrange
            _userBooksRepoMock.Setup(r => r.GetUserBooksByStatus(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new List<UserBook>());

            // Act
            var result = await _service.GetUserBooksByStatus(1, ReadingStatus.Reading);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(ErrorMessages.NoMoreBooks, result.Erros);
        }

        [Fact]
        public async Task GetUserBooksByStatus_ReturnsSuccess_WhenBooksExist()
        {
            // Arrange
            var ub = _fixture.CreateValidUserBook();
            ub.Book = new Book { Title = "Book1", Author = "Author1", ReleaseYear = 2020, Score = 5, ImageUrl = "" };
            _userBooksRepoMock.Setup(r => r.GetUserBooksByStatus(1, ReadingStatus.Reading))
                .ReturnsAsync(new List<UserBook> { ub });

            // Act
            var result = await _service.GetUserBooksByStatus(1, ReadingStatus.Reading);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.Single(result.Data);
            Assert.Equal(ub.Book.Title, result.Data[0].Title);
        }

        [Fact]
        public async Task GetUserBooksByStatus_ReturnsError_WhenExceptionThrown()
        {
            // Arrange
            _userBooksRepoMock.Setup(r => r.GetUserBooksByStatus(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("DB failure"));

            // Act
            var result = await _service.GetUserBooksByStatus(1, ReadingStatus.Reading);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("DB failure", result.Erros[0]);
        }
        #endregion

        #region AddUserBook
        [Theory]
        [InlineData("invalid")]
        [InlineData("")]
        [InlineData("lido23")]
        [InlineData("0")]
        [InlineData(null)]
        public async Task AddUserBook_ShouldReturnError_WhenStatusIsInvalid(string status)
        {
            // Arrange
            var userBookDto = new UserBookDTO
            {
                IdBook = 1,
                IdUser = 1,
                ReadingStatus = status,
                UserScore = 3
            };

            // Act
            var result = await _service.AddUserBook(userBookDto);

            // Assert
            Assert.Equal(ErrorMessages.UserBookStatusInvalid, result.Erros.First());
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
        }


        [Fact]
        public async Task AddUserBook_ReturnsError_WhenBookAlreadyExists()
        {
            // Arrange
            var existing = _fixture.CreateValidUserBook();
            _userBooksRepoMock.Setup(r => r.DoesUserBookExists(1, 1))
                .ReturnsAsync(existing);

            var dto = new UserBookDTO { IdUser = 1, IdBook = 1, ReadingStatus = ReadingStatus.Reading, UserScore = 4 };

            // Act
            var result = await _service.AddUserBook(dto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(ErrorMessages.UserBookAlreadyExists, result.Erros);
        }

        [Fact]
        public async Task AddUserBook_ReturnsCreated_WhenSuccessful()
        {
            // Arrange
            var dto = new UserBookDTO { IdUser = 1, IdBook = 1, ReadingStatus = ReadingStatus.Reading, UserScore = 4 };
            var book = new Book { Id = 1, Title = "B1", Author = "A1", ReleaseYear = 2020, Score = 5, ImageUrl = "" };

            _userBooksRepoMock.Setup(r => r.DoesUserBookExists(1, 1)).ReturnsAsync((UserBook)null);
            _genericRepoMock.Setup(r => r.InsertAsync(It.IsAny<UserBook>(), true))
                .Returns(Task.CompletedTask)
                .Callback<UserBook, bool>((ub, _) => ub.Book = book);

            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<Book>(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(book);

            _genericRepoMock.Setup(r => r.UpdateAsync(book, true)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddUserBook(dto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(dto.IdBook, result.Data.IdBook);
        }

        [Fact]
        public async Task AddUserBook_ReturnsError_WhenExceptionThrown()
        {
            // Arrange
            _userBooksRepoMock.Setup(r => r.DoesUserBookExists(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("DB fail"));

            var dto = new UserBookDTO { IdUser = 1, IdBook = 1, ReadingStatus = ReadingStatus.Reading, UserScore = 4 };

            // Act
            var result = await _service.AddUserBook(dto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("DB fail", result.Erros[0]);
        }
        #endregion

        #region UpdateUserBook
        [Fact]
        public async Task UpdateUserBook_ReturnsError_WhenInvalidStatus()
        {
            // Arrange
            var dto = new UserBookDTO { IdUser = 1, IdBook = 1, ReadingStatus = "invalid", UserScore = 4 };

            // Act
            var result = await _service.UpdateUserBook(dto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(ErrorMessages.UserBookStatusInvalid, result.Erros);
        }

        [Fact]
        public async Task UpdateUserBook_ReturnsError_WhenBookNotFound()
        {
            // Arrange
            _userBooksRepoMock.Setup(r => r.DoesUserBookExists(1, 1)).ReturnsAsync((UserBook)null);
            var dto = new UserBookDTO { IdUser = 1, IdBook = 1, ReadingStatus = ReadingStatus.Reading, UserScore = 4 };

            // Act
            var result = await _service.UpdateUserBook(dto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(ErrorMessages.BookNotFound, result.Erros);
        }

        [Fact]
        public async Task UpdateUserBook_ReturnsCreated_WhenSuccessful()
        {
            // Arrange
            var existing = _fixture.CreateValidUserBook();
            existing.Book = new Book { Id = 1, Title = "B1", Author = "A1", ReleaseYear = 2020, Score = 5 };
            _userBooksRepoMock.Setup(r => r.DoesUserBookExists(1, 1)).ReturnsAsync(existing);

            _genericRepoMock.Setup(r => r.UpdateAsync(existing, true)).Returns(Task.CompletedTask);
            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<Book>(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(existing.Book);
            _genericRepoMock.Setup(r => r.UpdateAsync(existing.Book, true)).Returns(Task.CompletedTask);

            var dto = new UserBookDTO { IdUser = 1, IdBook = 1, ReadingStatus = ReadingStatus.Read, UserScore = 4 };

            // Act
            var result = await _service.UpdateUserBook(dto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(dto.UserScore, result.Data.UserScore);
        }

        [Fact]
        public async Task UpdateUserBook_ReturnsError_WhenExceptionThrown()
        {
            // Arrange
            _userBooksRepoMock.Setup(r => r.DoesUserBookExists(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("DB fail"));
            var dto = new UserBookDTO { IdUser = 1, IdBook = 1, ReadingStatus = ReadingStatus.Reading, UserScore = 4 };

            // Act
            var result = await _service.UpdateUserBook(dto);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("DB fail", result.Erros[0]);
        }
        #endregion

        #region DeleteUserBook
        [Fact]
        public async Task DeleteUserBook_ReturnsError_WhenBookNotFound()
        {
            // Arrange
            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<UserBook>(It.IsAny<Expression<Func<UserBook, bool>>>()))
                .ReturnsAsync((UserBook)null);

            // Act
            var result = await _service.DeleteUserBook(1, 1);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(ErrorMessages.BookNotFound, result.Erros);
        }

        [Fact]
        public async Task DeleteUserBook_ReturnsSuccess_WhenBookExists()
        {
            // Arrange
            var ub = _fixture.CreateValidUserBook();
            ub.Book = new Book { Id = 1, Title = "B1", Author = "A1", ReleaseYear = 2020, Score = 5 };

            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<UserBook>(It.IsAny<Expression<Func<UserBook, bool>>>()))
                .ReturnsAsync(ub);
            _genericRepoMock.Setup(r => r.DeleteAsync(ub, true)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteUserBook(ub.IdUser, ub.IdBook);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(ub.Book.Title, result.Data.Title);
        }

        [Fact]
        public async Task DeleteUserBook_ReturnsError_WhenExceptionThrown()
        {
            // Arrange
            _genericRepoMock.Setup(r => r.GetFirstOrDefaultAsync<UserBook>(It.IsAny<Expression<Func<UserBook, bool>>>()))
                .ThrowsAsync(new Exception("DB fail"));

            // Act
            var result = await _service.DeleteUserBook(1, 1);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("DB fail", result.Erros[0]);
        }
        #endregion
    }
}
