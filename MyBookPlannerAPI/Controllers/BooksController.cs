using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookPlanner.Service.Interfaces;

namespace MyBookPlanner.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private IBooksService _booksService;
        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        [Route("/books")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBooks([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            var books = await _booksService.GetBooks(page, pageSize);
            return books.ToActionResult();
        }

        [HttpGet]
        [Route("/books/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var book = await _booksService.GetBookById(id);
            return book.ToActionResult();
        }
    }
}

