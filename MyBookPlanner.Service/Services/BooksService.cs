using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlanner.Domain.ViewModels.Books;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Service.Interfaces;
using MyBookPlanner.Utils.Mappers;

namespace MyBookPlanner.Service.Services
{
    public class BooksService : IBooksService
    {
        private IBooksRepository _booksRepository;
        public BooksService(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }
        public async Task<Result<BookViewModel>> GetBookById(int idBook)
        {
            try
            {
                var book = await _booksRepository.GetBookById(idBook);

                if(book is null)
                {
                    return Result<BookViewModel>.Error(ErrorMessages.BookNotFound);
                }

                return Result<BookViewModel>.Sucess(book.Convert());
            }
            catch (Exception ex)
            {
                return Result<BookViewModel>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }

        public async Task<Result<List<BookViewModel>>> GetBooks(int page, int pageSize)
        {
            try
            {
                // Books already comes in order of highest score for rankings.
                var books = await _booksRepository.GetBooks(page, pageSize);

                if(books is null || books.Count <= 0)
                {
                    return Result<List<BookViewModel>>.Error(ErrorMessages.NoMoreBooks);
                }

                return Result<List<BookViewModel>>.Sucess(books.ConvertList());
            }
            catch (Exception ex)
            {
                return Result<List<BookViewModel>>.Error(ErrorMessages.GenericError + ": " + ex.Message);
            }
        }
    }
}
