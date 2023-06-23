using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.Models;
using MyBookPlannerAPI.ViewModels;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlannerAPI.Controllers
{
    [ApiController]
    public class UserBooksController : ControllerBase
    {
        [HttpGet]
        [Route("/user-book/readed/{id:int}")]
        public async Task<IActionResult> GetUserReadedBooks([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            try
            {
                // The .join is for booksReaded to have both the users reading books and each book details.
                var booksReaded = await context.UserBooks.Where(x => x.IdUser == id && x.ReadingStatus.ToUpper() == "LIDO").Join(context.Books, userBook => userBook.IdBook, book => book.Id, (userBook, book) => new {
                    UserBook = userBook,
                    Book = book
                }).OrderByDescending(x => x.UserBook.UserScore).ToListAsync();


                //  This converts booksReaded to UserBooksViewModel
                var userBooksViewModelList = booksReaded.Select(x => new UserBooksViewModel
                {
                    IdUser = x.UserBook.IdUser,
                    IdBook = x.UserBook.IdBook,
                    UserScore = x.UserBook.UserScore,
                    ReadingStatus = x.UserBook.ReadingStatus,
                    Id = x.Book.Id,
                    Title = x.Book.Title,
                    Author = x.Book.Author,
                    ReleaseYear = x.Book.ReleaseYear,
                    ImageUrl = x.Book.ImageUrl,
                    Score = x.Book.Score
                })
                .ToList();

                if (booksReaded.Count == 0 || booksReaded == null)
                {
                    return NotFound(new ResultViewModel<UserBook>("User has not readed any books."));
                }

                return Ok(new ResultViewModel<List<UserBooksViewModel>>(userBooksViewModelList));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<UserBook>("Internal error."));
            }
        }

        [HttpGet]
        [Route("/user-book/reading/{id:int}")]
        public async Task<IActionResult> GetUserReadingBooks([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            try
            {
                // The .join is for booksReading to have both the users reading books and each book details.
                var booksReading = await context.UserBooks.Where(x => x.IdUser == id && x.ReadingStatus.ToUpper() == "LENDO").Join(context.Books, userBook => userBook.IdBook, book => book.Id, (userBook, book) => new {
                    UserBook = userBook,
                    Book = book
                }).OrderByDescending(x => x.UserBook.UserScore).ToListAsync();
              

                //  This converts booksReading to UserBooksViewModel
                var userBooksViewModelList = booksReading.Select(x => new UserBooksViewModel
                {
                    IdUser = x.UserBook.IdUser,
                    IdBook = x.UserBook.IdBook,
                    UserScore = x.UserBook.UserScore,
                    ReadingStatus = x.UserBook.ReadingStatus,
                    Id = x.Book.Id,
                    Title = x.Book.Title,
                    Author = x.Book.Author,
                    ReleaseYear = x.Book.ReleaseYear,
                    ImageUrl = x.Book.ImageUrl,
                    Score = x.Book.Score
                })
                .ToList();

                if (booksReading.Count == 0 || booksReading == null)
                {
                    return NotFound(new ResultViewModel<UserBook>("User has no reading books"));
                }

                return Ok(new ResultViewModel<List<UserBooksViewModel>>(userBooksViewModelList));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<UserBook>("Internal error."));
            }
        }


        [HttpGet]
        [Route("/user-book/wish-to-read/{id:int}")]
        public async Task<IActionResult> GetUserWishReads([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            try
            {
                var booksToRead = await context.UserBooks.Where(x => x.IdUser == id && x.ReadingStatus.ToUpper() == "DESEJO").Join(context.Books, userBook => userBook.IdBook, book => book.Id, (userBook, book) => new {
                    UserBook = userBook,
                    Book = book
                }).OrderByDescending(x => x.UserBook.UserScore).ToListAsync();


                //  This converts booksToRead to UserBooksViewModel
                var userBooksViewModelList = booksToRead.Select(x => new UserBooksViewModel
                {
                    IdUser = x.UserBook.IdUser,
                    IdBook = x.UserBook.IdBook,
                    UserScore = x.UserBook.UserScore,
                    ReadingStatus = x.UserBook.ReadingStatus,
                    Id = x.Book.Id,
                    Title = x.Book.Title,
                    Author = x.Book.Author,
                    ReleaseYear = x.Book.ReleaseYear,
                    ImageUrl = x.Book.ImageUrl,
                    Score = x.Book.Score
                })
                .ToList();

                if (booksToRead.Count == 0 || booksToRead == null)
                {
                    return NotFound(new ResultViewModel<UserBook>("User has no wish to list books."));
                }

                return Ok(new ResultViewModel<List<UserBooksViewModel>>(userBooksViewModelList));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<UserBook>("Internal error."));
            }
        }
    }
}
