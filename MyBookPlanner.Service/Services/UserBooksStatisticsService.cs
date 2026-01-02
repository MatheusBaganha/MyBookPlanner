using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Repository.Repositorys;
using MyBookPlanner.Service.Interfaces;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.Service.Services
{
    public class UserBooksStatisticsService : IUserBooksStatisticsService
    {
        private IGenericRepository _genericRepository;
        private IUserBooksRepository _userBooksRepository;
        public UserBooksStatisticsService(IGenericRepository genericRepository, IUserBooksRepository userBooksRepository)
        {
            _genericRepository = genericRepository;
            _userBooksRepository = userBooksRepository;
        }

        public async Task<Result<UserBestBookViewModel>> GetUserBestBook(int idUser)
        {
            try
            {
                var bestBook = await _userBooksRepository.GetUserBestBook(idUser);

                if (bestBook == null)
                {
                    // Generic book for new users that don't have books in their accounts.
                    var bestBookModelEmpty = new UserBestBookViewModel
                    {
                        Title = "MyBookPlannerBook",
                        Author = "Author",
                        ImageUrl = "",
                        ReleaseYear = DateTime.UtcNow.Year,
                        UserScore = 10,
                        IdUser = 0,
                        IdBook = 0
                    };

                    return Result<UserBestBookViewModel>.Sucess(bestBookModelEmpty);
                }


                return Result<UserBestBookViewModel>.Sucess(bestBook);

            }
            catch (Exception ex)
            {
                return Result<UserBestBookViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }

        public async Task<Result<UserBooksStatisticsViewModel>> GetUserStatistics(int idUser)
        {
            try
            {
                var userBooks = await _userBooksRepository.GetUserBooks(idUser);

                if (userBooks is null || userBooks.Count == 0)
                {
                    // When he is a new user in the catalog, he won't have any books to be calculated.
                    var statisticsOfRecentUser = new UserBooksStatisticsViewModel
                    {
                        reading = 0,
                        readed = 0,
                        wishToRead = 0
                    };
                    return Result<UserBooksStatisticsViewModel>.Sucess(statisticsOfRecentUser);
                }

                var readingBooks = userBooks.Count(x => x.ReadingStatus.ToUpper() == ReadingStatus.Reading);
                var readedBooks = userBooks.Count(x => x.ReadingStatus.ToUpper() == ReadingStatus.Read);
                var wishToReadBooks = userBooks.Count(x => x.ReadingStatus.ToUpper() == ReadingStatus.WishToRead);

                var statistics = new UserBooksStatisticsViewModel
                {
                    reading = readingBooks,
                    readed = readedBooks,
                    wishToRead = wishToReadBooks
                };


                return Result<UserBooksStatisticsViewModel>.Sucess(statistics);
            }
            catch (Exception ex)
            {
                return Result<UserBooksStatisticsViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }
    }
}
