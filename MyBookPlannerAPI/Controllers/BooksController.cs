using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Models;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.ViewModels;

namespace MyBookPlannerAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class BooksController : ControllerBase
    {

        [HttpGet]
        [Route("/books")]
        public async Task<IActionResult> GetBooks([FromServices] CatalogDataContext context, [FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Books already comes in order of highest score for rankings.
                var books = await context.Books.AsNoTracking().OrderByDescending(x => x.Score).Skip(page * pageSize).Take(pageSize).ToListAsync();

                if (books.Count() == 0 || books == null)
                {
                    return NotFound(new ResultViewModel<Book>("Books was not found."));
                }

                return Ok(new ResultViewModel<List<Book>>(books));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<Book>("Internal error."));
            }
        }

        [HttpGet]
        [Route("/books/{id:int}")]
        public async Task<IActionResult> GetBookById([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            try
            {
                var book = await context.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (book == null)
                {
                    return NotFound(new ResultViewModel<Book>("Book was not found."));
                }

                return Ok(new ResultViewModel<Book>(book));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<Book>("Internal error."));
            }
        }
    }
}

