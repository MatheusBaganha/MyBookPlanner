using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Repository.Data;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.Repository.Repositorys
{
    public class UserBooksRepository : IUserBooksRepository
    {
        private IGenericRepository _genericRepository;
        private MyBookPlannerDataContext  _context;
        public UserBooksRepository(IGenericRepository genericRepository, MyBookPlannerDataContext context)
        {
            _genericRepository = genericRepository;
            _context = context;
        }

        public async Task<UserBook> DoesUserBookExists(int idUser, int idBook)
        {
            var book = await _context.UserBooks.Include(ub => ub.Book).FirstOrDefaultAsync(x => x.IdUser == idUser && x.IdBook == idBook);
            return book;
        }


        public async Task<UserBestBookViewModel> GetUserBestBook(int idUser)
        {
            var bestBook = await _context.UserBooks
                .AsNoTracking()
                .Include(ub => ub.Book) 
                .Where(ub => ub.IdUser == idUser)
                .OrderByDescending(ub => ub.UserScore)
                .FirstOrDefaultAsync();

            if (bestBook == null) return null;

            return new UserBestBookViewModel
            {
                Title = bestBook.Book.Title,
                Author = bestBook.Book.Author,
                ImageUrl = bestBook.Book.ImageUrl,
                ReleaseYear = bestBook.Book.ReleaseYear,
                UserScore = bestBook.UserScore,
                IdUser = bestBook.IdUser,
                IdBook = bestBook.IdBook
            };
        }


        public async Task<List<UserBook>> GetUserBooks(int idUser)
        {
            var userBooks = await _genericRepository.GetList<UserBook>(x => x.IdUser == idUser).AsNoTracking().OrderByDescending(x => x.UserScore).ToListAsync();
            return userBooks;
        }

        public async Task<List<UserBook>> GetUserBooksByStatus(int idUser, string status)
        {
            var query = _context.UserBooks
                   .AsNoTracking()
                   .Where(x => x.IdUser == idUser);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.ReadingStatus.ToUpper() == status.ToUpper());
            }

            var books = await query
                .Include(x => x.Book)
                .OrderByDescending(x => x.UserScore)
                .ToListAsync();

            return books;

        }
    }
}
