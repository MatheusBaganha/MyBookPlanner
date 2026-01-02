using MyBookPlanner.Domain.ViewModels;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.Service.Interfaces
{
    public interface IUserBooksStatisticsService
    {
        public Task<Result<UserBooksStatisticsViewModel>> GetUserStatistics(int idUser);
        public Task<Result<UserBestBookViewModel>> GetUserBestBook(int idUser);
    }
}
