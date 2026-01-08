using System;
using MyBookPlanner.Domain.Models;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.Repository.Interfaces
{
    public interface IUserBooksRepository
    {
        public Task<List<UserBook>> GetUserBooks(int idUser);
        public Task<UserBestBookViewModel> GetUserBestBook(int idUser);
        public Task<List<UserBook>> GetUserBooksByStatus(int idUser, string status);
        public Task<UserBook> DoesUserBookExists(int idUser, int idBook);
    }
}
