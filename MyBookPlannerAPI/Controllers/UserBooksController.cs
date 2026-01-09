using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Service.Interfaces;

namespace MyBookPlanner.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserBooksController : ControllerBase
    {
        private IUserBooksService _userBooksService;
        public UserBooksController(IUserBooksService userBooksService)
        {
            _userBooksService = userBooksService;
        }

        [HttpGet]
        [Route("/user-book/{idUser:int}/{status}")]
        public async Task<IActionResult> GetUserReadedBooks([FromRoute] int idUser, [FromRoute] string status)
        {
            var books = await _userBooksService.GetUserBooksByStatus(idUser, status);
            return books.ToActionResult();
        }


        [HttpPost]
        [Route("/user-book/add-book")]
        public async Task<IActionResult> AddBook([FromBody] UserBook model)
        {
            var bookAdded = await _userBooksService.AddUserBook(model);
            return bookAdded.ToActionResult();
        }

        [HttpPut]
        [Route("/user-book/update-book")]
        public async Task<IActionResult> UpdateBook([FromBody] UserBook model)
        {
            var bookUpdated = await _userBooksService.UpdateUserBook(model);
            return bookUpdated.ToActionResult();
        }


        [HttpDelete]
        [Route("/user-book/delete-book/{idUser:int}/{idBook:int}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int idUser, [FromRoute] int idBook)
        {
            var bookDeleted = await _userBooksService.DeleteUserBook(idUser, idBook);
            return bookDeleted.ToActionResult();

        }
    }
}