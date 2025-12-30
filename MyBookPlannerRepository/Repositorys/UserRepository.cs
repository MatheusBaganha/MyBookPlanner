using System;
using System.Collections.Generic;
using System.Text;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Repository.Interfaces;

namespace MyBookPlanner.Repository.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private IGenericRepository _genericRepository;
        public UserRepository(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task CreateUser(User user)
        {
            await _genericRepository.InsertAsync<User>(user);
        }

        public async Task<User> DoesUserExists(string email)
        {
            var userExists = await _genericRepository.GetFirstOrDefaultAsync<User>(x => x.Email == email);
            return userExists;
        }

        public async Task<User> GetUser(string email, string passwordHash)
        {
            var user = await _genericRepository.GetFirstOrDefaultAsync<User>(x => x.Email == email && x.PasswordHash == passwordHash);
            return user;
        }
    }
}
