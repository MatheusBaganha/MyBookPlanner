using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Repository.Data;
using MyBookPlanner.Repository.Interfaces;

namespace MyBookPlanner.Repository.Repositorys
{
    public class BooksRepository : IBooksRepository
    {
        private IGenericRepository _genericRepository;
        public BooksRepository(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<List<Book>> GetBooks(int page = 0, int pageSize = 10)
        {
            var books = await _genericRepository.GetPagedList<Book>(page,
                                                                    pageSize,
                                                                    orderBy: x => x.OrderByDescending(x => x.Score))
                                                .AsNoTracking().ToListAsync();
            return books;
        }

        public async Task<Book> GetBookById(int idBook)
        {
            var book = await _genericRepository.GetFirstAsync<Book>(x => x.Id == idBook);
            return book;
        }
    }
}
