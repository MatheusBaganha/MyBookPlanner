using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Service.Interfaces;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserBooksStatisticsController : ControllerBase
    {
        private IUserBooksStatisticsService _userBooksStatisticsService;
        public UserBooksStatisticsController(IUserBooksStatisticsService userBooksStatisticsService)
        {
            _userBooksStatisticsService = userBooksStatisticsService;
        }

        [HttpGet]
        [Route("/user-book/{idUser:int}/statistics")]
        public async Task<IActionResult> GetUserStatistics([FromRoute] int idUser)
        {
            var userStatistics = await _userBooksStatisticsService.GetUserStatistics(idUser);
            return userStatistics.ToActionResult();
        }

        [HttpGet]
        [Route("/user-book/{idUser:int}/best-book")]
        public async Task<IActionResult> GetUserBestBook([FromRoute] int idUser)
        {
            var book = await _userBooksStatisticsService.GetUserBestBook(idUser);
            return book.ToActionResult();
        }
    }
}
