using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyBookPlanner.Domain.Config;
using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.DTO;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlanner.Domain.ViewModels.Users;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Service.Interfaces;
using Newtonsoft.Json.Linq;
using SecureIdentity.Password;

namespace MyBookPlanner.Service.Services
{
    public class AuthService : IAuthService
    {
        private IGenericRepository _genericRepository;
        private IUserRepository _userRepository;
        private TokenService _tokenService;
        public AuthService(IGenericRepository genericRepository, IUserRepository userRepository, TokenService tokenService)
        {
            _genericRepository = genericRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<Result<UserViewModel>> Login(UserLoginDTO user)
        {
            try
            {
                var userFound = await _userRepository.DoesUserExists(user.Email);

                if (userFound is null)
                {
                    return Result<UserViewModel>.Unauthorized(ErrorMessages.InvalidUser);
                }

                if (!PasswordHasher.Verify(userFound.PasswordHash, user.Password))
                {
                    return Result<UserViewModel>.Unauthorized(ErrorMessages.InvalidUser);
                }

                var token = _tokenService.GenerateToken(userFound);

                var userViewModel = new UserViewModel()
                {
                    Email = userFound.Email,
                    Biography = userFound.Biography,
                    UserToken = token,
                };

                return Result<UserViewModel>.Sucess(userViewModel);
            }
            catch (Exception ex)
            {
                return Result<UserViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }

        public async Task<Result<UserViewModel>> Register(RegisterUserDTO user)
        {
            try
            {
                var userAlreadyExists = await _userRepository.DoesUserExists(user.Email);

                if (userAlreadyExists != null)
                {
                    return Result<UserViewModel>.Conflict(ErrorMessages.UserEmailAlreadyExists);
                }

                var userToCreate = new User
                {
                    // Id is generated automatically in DB and Biography is default value.
                    Id = 0,
                    Biography = UtilMessages.DefaultBiography,
                    Username = user.Username,
                    Email = user.Email,
                    PasswordHash = PasswordHasher.Hash(user.Password),
                };


                await _userRepository.CreateUser(userToCreate);
                var userCreated = await _genericRepository.SaveChangesAsync();

                if(!userCreated)
                {
                    return Result<UserViewModel>.Error(ErrorMessages.ErrorOnCreating);
                }

                var token = _tokenService.GenerateToken(userToCreate);

                var userViewModel = new UserViewModel()
                {
                    Email = userToCreate.Email,
                    Biography = userToCreate.Biography,
                    UserToken = token
                };

                return Result<UserViewModel>.Created(userViewModel);
            }
            catch (Exception ex)
            {
                return Result<UserViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }


      
    }
}
