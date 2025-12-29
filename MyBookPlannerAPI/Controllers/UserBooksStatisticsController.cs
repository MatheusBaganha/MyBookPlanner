using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Domain.Models;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.ViewModels;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    public class UserBooksStatisticsController : ControllerBase
    {
        [HttpGet]
        [Route("/user-book/{idUser:int}/statistics")]
        public async Task<IActionResult> GetUserStatistics([FromServices] CatalogDataContext context, [FromRoute] int idUser)
        {
            try
            {
                var userBooks = await context.UserBooks.AsNoTracking().Where(x => x.IdUser == idUser).ToListAsync();

                if(userBooks.Count == 0 || userBooks == null)
                {
                    // When the is a new user in the catalog, he won't have any books to be calculated.
                    var statisticsOfRecentUser = new UserBooksStatisticsViewModel
                    {
                        reading = 0,
                        readed = 0,
                        wishToRead = 0
                    };
                    return Ok(new ResultViewModel<UserBooksStatisticsViewModel>(statisticsOfRecentUser));
                }

                var readingBooks = userBooks.Count(x => x.ReadingStatus.ToUpper() == "LENDO");
                var readedBooks = userBooks.Count(x => x.ReadingStatus.ToUpper() == "LIDO");
                var wishToReadBooks = userBooks.Count(x => x.ReadingStatus.ToUpper() == "DESEJO");

                var statistics = new UserBooksStatisticsViewModel
                {
                    reading = readingBooks,
                    readed = readedBooks,
                    wishToRead = wishToReadBooks
                };

                return Ok(new ResultViewModel<UserBooksStatisticsViewModel>(statistics));
            } 
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<UserBooksStatisticsViewModel>("Internal error."));
            }
        }

        [HttpGet]
        [Route("/user-book/{idUser:int}/best-book")]
        public async Task<IActionResult> GetUserBestBook([FromServices] CatalogDataContext context, [FromRoute] int idUser)
        {
            try
            {
                // The .join is for bestBook to have both the user book and the book details.
                // It will bring us the book with the user highest score.
                var bestBook = await context.UserBooks.AsNoTracking().Where(x => x.IdUser == idUser).Join(context.Books, userBook => userBook.IdBook, book => book.Id, (userBook, book) => new
                {
                    UserBook = userBook,
                    Book = book
                }).OrderByDescending(x => x.UserBook.UserScore).FirstOrDefaultAsync();

                if (bestBook == null)
                {
                    // Generic book for new users that don't have books in their accounts.
                    var bestBookModelEmpty = new UserBestBookViewModel
                    {
                        Title = "MyBookPlannerBook",
                        Author = "Author MBP",
                        ImageUrl = "https://drive.google.com/uc?export=view&id=1UuXzpVHAUeBW3o1YTVyprNkJ8FPM6MHW",
                        ReleaseYear = DateTime.UtcNow.Year,
                        UserScore = 10,
                        IdUser = 0,
                        IdBook = 0
                    };
                    return Ok(new ResultViewModel<UserBestBookViewModel>(bestBookModelEmpty));
                }

                //  This converts bestBook to UserBestBooks type.
                var bestBookModel = new UserBestBookViewModel
                {
                    Title = bestBook.Book.Title,
                    Author = bestBook.Book.Author,
                    ImageUrl = bestBook.Book.ImageUrl,
                    ReleaseYear = bestBook.Book.ReleaseYear,
                    UserScore = bestBook.UserBook.UserScore,
                    IdUser = bestBook.UserBook.IdUser,
                    IdBook = bestBook.UserBook.IdBook
                };

                return Ok(new ResultViewModel<UserBestBookViewModel>(bestBookModel));
            }
            catch (Exception ex)
            {
                return BadRequest("Internal error.");
            }
        }
    }
}
