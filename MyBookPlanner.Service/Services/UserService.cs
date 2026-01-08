using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlanner.Domain.ViewModels.Users;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Service.Interfaces;
using MyBookPlanner.Utils;
using MyBookPlanner.Utils.Mappers;

namespace MyBookPlanner.Service.Services
{
    public class UserService : IUserService
    {
        private IGenericRepository _genericRepository;
        private IUserRepository _userRepository;
        public UserService(IGenericRepository genericRepository, IUserRepository userRepository)
        {
            _genericRepository = genericRepository;
            _userRepository = userRepository;
        }
        public async Task<Result<UserViewModel>> GetUserById(int idUser)
        {
            try
            {
                var user = await _genericRepository.GetFirstOrDefaultAsync<User>(x => x.Id == idUser);

                if (user == null)
                {
                    return Result<UserViewModel>.Error(ErrorMessages.UserNotFound);
                }

                return Result<UserViewModel>.Sucess(user.Convert());
            }
            catch (Exception ex)
            {
                return Result<UserViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }

        public async Task<Result<UserViewModel>> UpdateUser(int idUser, UpdateUserDTO model)
        {
            try
            {
                var user = await _genericRepository.GetFirstOrDefaultAsync<User>(x => x.Id == idUser);

                if (user is null)
                {
                    return Result<UserViewModel>.Error(ErrorMessages.UserNotFound);
                }

                var emailAlreadyInUse = await _userRepository.DoesUserExists(model.Email);

                if (emailAlreadyInUse != null)
                {
                    return Result<UserViewModel>.Conflict(ErrorMessages.UserEmailAlreadyExists);
                }

                user.Username = model.Username;
                user.Email = model.Email;
                user.Biography = model.Biography;

                await _genericRepository.UpdateAsync<User>(user);
                var userUpdated = await _genericRepository.SaveChangesAsync();

                if(!userUpdated)
                {
                    return Result<UserViewModel>.Error(ErrorMessages.ErrorOnUpdating);
                }

                return Result<UserViewModel>.Sucess(user.Convert());
            }
            catch (Exception ex)
            {
                return Result<UserViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }

        public async Task<Result<UserViewModel>> DeleteUser(int idUser)
        {
            try
            {
                var user = await _genericRepository.GetFirstOrDefaultAsync<User>(x => x.Id == idUser);

                if (user is null)
                {
                    return Result<UserViewModel>.Error(ErrorMessages.UserNotFound);
                }

                await _genericRepository.DeleteAsync<User>(user);
                var userDeleted = await _genericRepository.SaveChangesAsync();

                if (!userDeleted)
                {
                    return Result<UserViewModel>.Error(ErrorMessages.ErrorOnCreating);
                }

                return Result<UserViewModel>.Sucess(user.Convert());
            }
            catch (Exception ex)
            {
                return Result<UserViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);

            }
        }


    
    }
}
