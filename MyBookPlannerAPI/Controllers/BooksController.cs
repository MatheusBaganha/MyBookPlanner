using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Models;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;

namespace MyBookPlannerAPI.Controllers
{
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        [Route("/books")]
        public async Task<IActionResult> GetBooksAsync([FromServices] CatalogDataContext context)
        {
            try
            {
                // Books already comes in order of highest score for rankings.
                var books = await context.Books.OrderByDescending(x => x.Score).ToListAsync();

                if (books == null)
                {
                    return NotFound(new ResultViewModel<Book>("Books was not found."));
                }

                return Ok(new ResultViewModel<List<Book>>(books));
            }

            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Book>("Internal error."));
            }
        }

        [HttpGet]
        [Route("/books/{id:int}")]
        public async Task<IActionResult> GetBookById([FromServices] CatalogDataContext context, [FromRoute] int id)
        {
            try
            {
                var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

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

