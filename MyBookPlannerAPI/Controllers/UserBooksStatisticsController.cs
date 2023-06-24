using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.ViewModels;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlannerAPI.Controllers
{
    public class UserBooksStatisticsController : ControllerBase
    {
        [HttpGet]
        [Route("/user-book/{idUser:int}/statistics")]
        public async Task<IActionResult> GetUserStatistics([FromServices] CatalogDataContext context, [FromRoute] int idUser)
        {
            try
            {
                var userBooks = await context.UserBooks.Where(x => x.IdUser == idUser).ToListAsync();

                if(userBooks.Count == 0 || userBooks == null)
                {
                    return NotFound(new ResultViewModel<UserBooksStatisticsViewModel>("User books was not found."));
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
    }
}
