using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlanner.Repository.Data;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Service.Interfaces;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.Service.Services
{
    public class UserBooksService : IUserBooksService
    {
        private MyBookPlannerDataContext _context;
        private IGenericRepository _genericRepository;
        private IUserBooksRepository _userBooksRepository;
        public UserBooksService(MyBookPlannerDataContext context, IGenericRepository genericRepository, IUserBooksRepository userBooksRepository)
        {
            _context = context;
            _userBooksRepository = userBooksRepository;
            _genericRepository = genericRepository;
        }

        public async Task<Result<List<UserBooksViewModel>>> GetUserBooksByStatus(int idUser, string status)
        {
            try
            {
                var books = await _userBooksRepository.GetUserBooksByStatus(idUser, status);

                if (books is null || books.Count == 0)
                {
                    return Result<List<UserBooksViewModel>>.Error(ErrorMessages.NoMoreBooks);
                }

                var userBooksViewModelList = books.Select(x => new UserBooksViewModel
                {
                    IdUser = x.IdUser,
                    IdBook = x.IdBook,
                    UserScore = x.UserScore,
                    ReadingStatus = x.ReadingStatus,
                    Title = x.Book.Title,
                    Author = x.Book.Author,
                    ReleaseYear = x.Book.ReleaseYear,
                    ImageUrl = x.Book.ImageUrl,
                    Score = x.Book.Score
                })
                .ToList();

                return Result<List<UserBooksViewModel>>.Sucess(userBooksViewModelList);
            }
            catch (Exception ex)
            {
                return Result<List<UserBooksViewModel>>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }

        public async Task<Result<UserBooksViewModel>> AddUserBook(UserBook model)
        {
            try
            {
                var userAlreadyHasBook = await _userBooksRepository.DoesUserBookExists(model.IdUser, model.IdBook);

                if (userAlreadyHasBook is not null)
                {
                    return Result<UserBooksViewModel>.Error(ErrorMessages.UserBookAlreadyExists);
                }

                var userBook = new UserBook
                {
                    IdBook = model.IdBook,
                    IdUser = model.IdUser,
                    ReadingStatus = model.ReadingStatus,
                    UserScore = float.Parse(model.UserScore.ToString("0.0"))
                };


                await  _context.UserBooks.AddAsync(userBook);
                var userBookAdded = await _context.SaveChangesAsync() > 0;

                if(!userBookAdded)
                {
                    return Result<UserBooksViewModel>.Error(ErrorMessages.ErrorOnCreating);
                }

                var averageScoreUpdated = await UpdateAverageBookScore(userBook);
                if(!averageScoreUpdated)
                {
                    // Save log data to fix it later if it happens. But it will not be made in this project, maybe in the future, or not (just lazyness).
                }

                var userBookViewModel = new UserBooksViewModel()
                {
                    IdBook = model.IdBook,
                    IdUser = model.IdUser,
                    Author = userBook.Book.Author,
                    ImageUrl = userBook.Book.ImageUrl,
                    ReadingStatus = userBook.ReadingStatus,
                    ReleaseYear = userBook.Book.ReleaseYear,
                    Score = userBook.Book.Score,
                    Title = userBook.Book.Title,
                    UserScore = userBook.UserScore,
                };


                return Result<UserBooksViewModel>.Created(userBookViewModel);
            }
            catch (Exception ex)
            {
                return Result<UserBooksViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);

            }
        }

        public async Task<Result<UserBooksViewModel>> UpdateUserBook(UserBook model)
        {
            try
            {
                var userHasBook = await _userBooksRepository.DoesUserBookExists(model.IdUser, model.IdBook);


                if (userHasBook is null)
                {
                    return Result<UserBooksViewModel>.Error(ErrorMessages.BookNotFound);
                }

                userHasBook.IdBook = model.IdBook;
                userHasBook.IdUser = model.IdUser;
                userHasBook.ReadingStatus = model.ReadingStatus;
                userHasBook.UserScore = float.Parse(model.UserScore.ToString("0.0"));


                _context.UserBooks.Update(userHasBook);

                var userBookUpdated = await _context.SaveChangesAsync() > 0;

                if (!userBookUpdated)
                {
                    return Result<UserBooksViewModel>.Error(ErrorMessages.ErrorOnUpdating);
                }

                var averageScoreUpdated = await UpdateAverageBookScore(userHasBook);
                if (!averageScoreUpdated)
                {
                    // Save log data to fix it later if it happens. But it will not be made in this project, maybe in the future, or not (just lazyness).
                }


                var userBookViewModel = new UserBooksViewModel()
                {
                    IdBook = model.IdBook,
                    IdUser = model.IdUser,
                    Author = userHasBook.Book.Author,
                    ImageUrl = userHasBook.Book.ImageUrl,
                    ReadingStatus = userHasBook.ReadingStatus,
                    ReleaseYear = userHasBook.Book.ReleaseYear,
                    Score = userHasBook.Book.Score,
                    Title = userHasBook.Book.Title,
                    UserScore = userHasBook.UserScore,
                };


                return Result<UserBooksViewModel>.Created(userBookViewModel);
            }
            catch (Exception ex)
            {
                return Result<UserBooksViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);

            }
        }


        public async Task<Result<UserBooksViewModel>> DeleteUserBook(int idUser, int idBook)
        {
            try
            {
                var userBook = await _context.UserBooks.FirstOrDefaultAsync(x => x.IdUser == idUser && x.IdBook == idBook);

                if (userBook is null)
                {
                    return Result<UserBooksViewModel>.Error(ErrorMessages.BookNotFound);
                }


                var userBookViewModel = new UserBooksViewModel()
                {
                    IdBook = userBook.IdBook,
                    IdUser = userBook.IdUser,
                    Author = userBook.Book.Author,
                    ImageUrl = userBook.Book.ImageUrl,
                    ReadingStatus = userBook.ReadingStatus,
                    ReleaseYear = userBook.Book.ReleaseYear,
                    Score = userBook.Book.Score,
                    Title = userBook.Book.Title,
                    UserScore = userBook.UserScore,
                };


                await _context.SaveChangesAsync();

                return Result<UserBooksViewModel>.Sucess(userBookViewModel);
            }
      
            catch (Exception ex)
            {
                return Result<UserBooksViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }


        // This will update the average book score.
        // It will only update when the user already had readed the book.
        private async Task<bool> UpdateAverageBookScore(UserBook model)
        {
            Book? book;

            if (model.ReadingStatus.ToUpper() == ReadingStatus.Read)
            {
                book = await _context.Books.FirstOrDefaultAsync(x => x.Id == model.IdBook);

                if (book != null)
                {
                    book.Score = (book.Score + model.UserScore) / 2;
                    _context.Books.Update(book);
                }

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }
    }
}
