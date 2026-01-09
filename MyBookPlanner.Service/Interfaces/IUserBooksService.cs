using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.Service.Interfaces
{
    public interface IUserBooksService
    {
        Task<Result<List<UserBooksViewModel>>> GetUserBooksByStatus(int idUser, string status);
        Task<Result<UserBooksViewModel>> AddUserBook(UserBookDTO model);
        Task<Result<UserBooksViewModel>> UpdateUserBook(UserBookDTO model);
        Task<Result<UserBooksViewModel>> DeleteUserBook(int idUser, int idBook);
    }
}
