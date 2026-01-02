using System;
using MyBookPlanner.Domain.Models;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.Repository.Interfaces
{
    public interface IUserBooksRepository
    {
        public Task<List<UserBook>> GetUserBooks(int idUser);
        public Task<UserBestBookViewModel> GetUserBestBook(int idUser);
    }
}
