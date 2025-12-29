using MyBookPlanner.Domain.Models;

namespace MyBookPlanner.Repository.Interfaces
{
    public interface IBooksRepository
    {
        Task<List<Book>> GetBooks(int page = 0, int pageSize = 10);
        Task<Book> GetBookById(int idBook);
    }
}
