using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Service.Interfaces;

namespace MyBookPlanner.Service.Services
{
    public class BooksService : IBooksService
    {
        private IBooksRepository _booksRepository;
        public BooksService(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }
        public async Task<Result<Book>> GetBookById(int idBook)
        {
            try
            {
                var book = await _booksRepository.GetBookById(idBook);

                if(book is null)
                {
                    return Result<Book>.Error(ErrorMessages.BookNotFound);
                }

                return Result<Book>.Sucess(book);
            }
            catch (Exception ex)
            {
                return Result<Book>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }

        public async Task<Result<List<Book>>> GetBooks(int page, int pageSize)
        {
            try
            {
                // Books already comes in order of highest score for rankings.
                var books = await _booksRepository.GetBooks(page, pageSize);

                if(books is null || books.Count <= 0)
                {
                    return Result<List<Book>>.Error(ErrorMessages.NoMoreBooks);
                }

                return Result<List<Book>>.Sucess(books);
            }
            catch (Exception ex)
            {
                return Result<List<Book>>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }
    }
}
