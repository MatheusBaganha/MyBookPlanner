using Microsoft.AspNetCore.Mvc;
using MyBookPlanner.Models;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.ViewModels.UserBooks;
using MyBookPlannerAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MyBookPlannerAPI.Controllers
{
    [ApiController]
    public class CatalogController : ControllerBase
    {
        [HttpGet]
        [Route("/catalog/{userId:int}")]
        public async Task<IActionResult> GetBooksAsync([FromServices] CatalogDataContext context, [FromRoute] int userId)
        {
            try
            {
                // Books already comes in order of highest score for rankings.
                var books = await context.Books.AsNoTracking().OrderByDescending(x => x.Score).ToListAsync();

                // Prevents the DB to receive another query from allUserBooks.
                if (books == null)
                {
                    return NotFound(new ResultViewModel<Book>("Books was not found."));
                }

                // The .join is for allUserBooks to have both the users books and each book details.
                var allUserBooks = await context.UserBooks.Where(x => x.IdUser == userId).Join(context.Books, userBook => userBook.IdBook, book => book.Id, (userBook, book) => new {
                    UserBook = userBook,
                    Book = book
                }).OrderByDescending(x => x.UserBook.UserScore).ToListAsync();

                if (allUserBooks == null)
                {
                    return NotFound(new ResultViewModel<Book>("User books was not found."));
                }


                // This is the list that will have the books lists
                // but in the CatalogViewModel type
                List<CatalogViewModel> booksInCatalogModel = new List<CatalogViewModel>();

                // This list is selecting the items that are necessary to make this list
                // a list of CatalogViewModel.
                List<CatalogViewModel> allUserBooksInCatalogModel = allUserBooks.Select(x => new CatalogViewModel
                {
                    IdUser = x.UserBook.IdUser,
                    IdBook = x.Book.Id,
                    Author = x.Book.Author,
                    Title = x.Book.Title,
                    ImageUrl = x.Book.ImageUrl,
                    ReleaseYear = x.Book.ReleaseYear,
                    Score = x.Book.Score,
                    UserScore = x.UserBook.UserScore,
                    ReadingStatus = x.UserBook.ReadingStatus
                }).ToList();


                // Here we are inserting UserScore and ReadingStatus in the generic books
                // that comes from the database to match the CatalogViewModel type.
                foreach (var book in books)
                {
                    var bookInCatalogModel = new CatalogViewModel
                    {
                        IdUser = 0,
                        IdBook = book.Id,
                        Author = book.Author,
                        Title = book.Title,
                        ImageUrl = book.ImageUrl,
                        ReleaseYear = book.ReleaseYear,
                        Score = book.Score,
                        UserScore = 0,
                        ReadingStatus = "None"
                    };

                    // Then we add it to the new list we have created.
                    booksInCatalogModel.Add(bookInCatalogModel);
                }

                // Then we combine both lists. ToList() is important to have because
                // if we remove it, it will be an IEnumerable and we won't be able
                // to use the class that removes the duplicated values.
                var combinedBooks = booksInCatalogModel.Concat(allUserBooksInCatalogModel).ToList();


                // Here we have the list that will in fact be returned
                // This list is already filtered to have the users books and its userScore
                // together with the generic books. 
                // It helps the front-end to directly creates the items in the screen
                // without the need to worry about mixing the user books and the generic books.
                var catalogList = DuplicatedItemsInCatalogListsRemover.RemoveDuplicates(combinedBooks);

                return Ok(new ResultViewModel<List<CatalogViewModel>>(catalogList));
            }

            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Book>("Internal error."));
            }
        }
    }
}
