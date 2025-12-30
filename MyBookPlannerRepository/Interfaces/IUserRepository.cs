using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels.Users;

namespace MyBookPlanner.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUser(string email, string passwordHash);
        public Task<User> DoesUserExists(string email);
        public Task CreateUser(User user);
    }
}
