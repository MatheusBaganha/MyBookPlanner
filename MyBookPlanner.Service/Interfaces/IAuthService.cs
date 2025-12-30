using System;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlanner.Domain.ViewModels.Users;

namespace MyBookPlanner.Service.Interfaces
{
    public interface IAuthService
    {
        public Task<Result<UserViewModel>> Login(UserLoginDTO user);
        public Task<Result<UserViewModel>> Register(RegisterUserDTO user);
    }
}
