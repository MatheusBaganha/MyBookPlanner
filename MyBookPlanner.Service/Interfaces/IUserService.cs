using System;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlanner.Domain.ViewModels.Users;

namespace MyBookPlanner.Service.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserViewModel>> GetUserById(int idUser);
        Task<Result<UserViewModel>> UpdateUser(int idUser, UpdateUserDTO model);
        Task<Result<UserViewModel>> DeleteUser(int idUseriD);
    }
}
