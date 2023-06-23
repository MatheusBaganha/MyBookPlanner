using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.Models;
using MyBookPlannerAPI.ViewModels;

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
                var booksReaded = await context.UserBooks.Where(x => x.IdUser == id && x.ReadingStatus == "LIDO").OrderByDescending(x => x.UserScore).ToListAsync();
                
                if(booksReaded.Count == 0 || booksReaded == null)
                {
                    return NotFound(new ResultViewModel<UserBook>("User has not readed any books."));
                }

                return Ok(new ResultViewModel<List<UserBook>>(booksReaded));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultViewModel<UserBook>("Internal error."));
            }
        }
    }
}
