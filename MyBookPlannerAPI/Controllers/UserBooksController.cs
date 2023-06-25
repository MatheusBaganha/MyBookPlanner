using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Models;
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
                var booksReaded = await context.UserBooks.AsNoTracking().Where(x => x.IdUser == id && x.ReadingStatus.ToUpper() == "LIDO").Join(context.Books, userBook => userBook.IdBook, book => book.Id, (userBook, book) => new {
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
                var booksReading = await context.UserBooks.AsNoTracking().Where(x => x.IdUser == id && x.ReadingStatus.ToUpper() == "LENDO").Join(context.Books, userBook => userBook.IdBook, book => book.Id, (userBook, book) => new {
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
                var booksToRead = await context.UserBooks.AsNoTracking().Where(x => x.IdUser == id && x.ReadingStatus.ToUpper() == "DESEJO").Join(context.Books, userBook => userBook.IdBook, book => book.Id, (userBook, book) => new {
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


        [HttpGet]
        [Route("/user-book/all-books/{id:int}")]
        public async Task<IActionResult> GetUserAllBooks([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            try
            {
                // The .join is for booksReaded to have both the users reading books and each book details.
                var allBooks = await context.UserBooks.AsNoTracking().Where(x => x.IdUser == id).Join(context.Books, userBook => userBook.IdBook, book => book.Id, (userBook, book) => new {
                    UserBook = userBook,
                    Book = book
                }).OrderByDescending(x => x.UserBook.UserScore).ToListAsync();


                //  This converts allBooks to UserBooksViewModel
                var userBooksViewModelList = allBooks.Select(x => new UserBooksViewModel
                {
                    IdUser = x.UserBook.IdUser,
                    IdBook = x.UserBook.IdBook,
                    UserScore = x.UserBook.UserScore,
                    ReadingStatus = x.UserBook.ReadingStatus,
                    Title = x.Book.Title,
                    Author = x.Book.Author,
                    ReleaseYear = x.Book.ReleaseYear,
                    ImageUrl = x.Book.ImageUrl,
                    Score = x.Book.Score
                })
                .ToList();

                if (allBooks.Count == 0 || allBooks == null)
                {
                    return NotFound(new ResultViewModel<UserBook>("User has not added any books."));
                }

                return Ok(new ResultViewModel<List<UserBooksViewModel>>(userBooksViewModelList));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<UserBook>("Internal error."));
            }
        }


        [HttpPost]
        [Route("/user-book/add-book")]
        public async Task<IActionResult> AddBook([FromServices] CatalogDataContext context, [FromBody] UserBook model)
        {
            try
            {
                var userAlreadyHasBook = await context.UserBooks.FirstOrDefaultAsync(x => x.IdUser == model.IdUser && x.IdBook == model.IdBook);

                if(userAlreadyHasBook != null)
                {
                    return StatusCode(409, new ResultViewModel<UserBook>("User already has that book."));
                }
                var userBook = new UserBook
                {
                    IdBook = model.IdBook,
                    IdUser = model.IdUser,
                    ReadingStatus = model.ReadingStatus,
                    UserScore = model.UserScore
                };

                await context.UserBooks.AddAsync(userBook);
                await context.SaveChangesAsync();

                return Created($"/user-book/{userBook.IdUser}/{userBook.IdBook}", userBook);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<UserBook>("It was not possible to add the book."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<UserBook>("Internal error."));
            }
        }

        [HttpPut]
        [Route("/user-book/update-book")]
        public async Task<IActionResult> UpdateBook([FromServices] CatalogDataContext context, [FromBody] UserBook model)
        {
            try
            {
                var book = await context.UserBooks.FirstOrDefaultAsync(x => x.IdUser == model.IdUser && x.IdBook == model.IdBook);

                if(book == null)
                {
                    return NotFound(new ResultViewModel<UserBook>("User book was not found."));
                }

                book.IdBook = model.IdBook;
                book.IdUser = model.IdUser;
                book.ReadingStatus = model.ReadingStatus;
                book.UserScore = float.Parse(model.UserScore.ToString("0.0"));

                context.UserBooks.Update(book);
                await context.SaveChangesAsync();

                return Created($"/user-book/{book.IdUser}/{book.IdBook}", book);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<UserBook>("It was not possible to update the book."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<UserBook>("Internal error."));
            }
        }


        [HttpDelete]
        [Route("/user-book/delete-book/{idUser:int}/{idBook:int}")]
        public async Task<IActionResult> DeleteBook([FromServices] CatalogDataContext context, [FromRoute] int idUser, [FromRoute] int idBook)
        {
            try
            {
                var book = await context.UserBooks.FirstOrDefaultAsync(x => x.IdUser == idUser && x.IdBook == idBook);

                if (book == null)
                {
                    return NotFound(new ResultViewModel<UserBook>("User book was not found."));
                }

                context.UserBooks.Remove(book);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<UserBook>(book));
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new ResultViewModel<User>("It was not possible to delete the user book."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<User>("Internal error."));
            }
            
        }
    }
}