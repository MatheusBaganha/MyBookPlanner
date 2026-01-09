using System;
using System.Collections.Generic;
using System.Text;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;
using MyBookPlanner.Domain.ViewModels.Books;

namespace MyBookPlanner.Service.Interfaces
{
    public interface IBooksService
    {
        public Task<Result<List<BookViewModel>>> GetBooks(int page = 0, int pageSize = 10);
        public Task<Result<BookViewModel>> GetBookById(int idBook);
    }    
}
