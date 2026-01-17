using System.Net;
using Moq;
using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Service.Services;
using MyBookPlanner.Tests.Fixtures;

namespace MyBookPlanner.Tests.Services
{
    public class BookServiceTests
    {
        private readonly BookFixture _bookFixture;
        private readonly Mock<IBooksRepository> _bookRepoMock;
        private readonly BooksService _service;


        public BookServiceTests()
        {
            _bookFixture = new BookFixture();
            _bookRepoMock = new Mock<IBooksRepository>();
            _service = new BooksService(_bookRepoMock.Object);
        }

        #region GetUserById

        [Fact]
        public async Task GetBookById_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var book = _bookFixture.CreateValidBook();
            _bookRepoMock.Setup(x => x.GetBookById(book.Id)).ReturnsAsync(book);

            // Act
            var result = await _service.GetBookById(book.Id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(book.Id, result.Data.Id);
            Assert.Equal(book.Title, result.Data.Title);
            Assert.Equal(book.Author, result.Data.Author);
            Assert.Equal(book.ImageUrl, result.Data.ImageUrl);
            Assert.Equal(book.ReleaseYear, result.Data.ReleaseYear);
            Assert.Equal((float)Math.Round(book.Score, 1), book.Score);  //check that the score has only 1 decimal place
        }


        [Fact]
        public async Task GetBookById_ShouldReturnError_WhenBookIsNull()
        {
            // Arrange
            _bookRepoMock.Setup(x => x.GetBookById(1)).ReturnsAsync((Book)null);

            // Act
            var result = await _service.GetBookById(1);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(ErrorMessages.BookNotFound, result.Erros);
        }

        [Fact]
        public async Task GetBookById_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            _bookRepoMock.Setup(x => x.GetBookById(1)).ThrowsAsync(new Exception("Database down"));

            // Act
            var result = await _service.GetBookById(1);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("Database down", result.Erros[0]);
        }


        #endregion 


        #region GetBooks

        [Fact]
        public async Task GetBooks_ShouldReturnBooks_WhenBooksExist()
        {
            // Arrange
            var books = _bookFixture.CreateBookList(5);
            _bookRepoMock.Setup(r => r.GetBooks(1, 5)).ReturnsAsync(books);

            // Act 
            var result = await _service.GetBooks(1, 5);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(5, result.Data.Count);

            // Assert 
            for (int i = 0; i < books.Count; i++)
            {
                var book = books[i];
                var vm = result.Data[i];

                Assert.Equal(book.Id, vm.Id);
                Assert.Equal(book.Title, vm.Title);
                Assert.Equal(book.Author, vm.Author);
                Assert.Equal(book.ImageUrl, vm.ImageUrl);
                Assert.Equal(book.ReleaseYear, vm.ReleaseYear);

                // check that the score has only 1 decimal place
                Assert.Equal((float)Math.Round(book.Score, 1), book.Score);
            }
        }

        [Fact]
        public async Task GetBooks_ShouldReturnError_WhenNoBooksExist()
        {
            // Arrange
            _bookRepoMock.Setup(r => r.GetBooks(1, 5)).ReturnsAsync(new List<Book>());

            // Act
            var result = await _service.GetBooks(1, 5);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(ErrorMessages.NoMoreBooks, result.Erros);
        }

        [Fact]
        public async Task GetBooks_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            _bookRepoMock.Setup(r => r.GetBooks(1, 5)).ThrowsAsync(new Exception("Database down"));

            // Act
            var result = await _service.GetBooks(1, 5);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("Database down", result.Erros[0]);
        }
        #endregion 

    }
}
