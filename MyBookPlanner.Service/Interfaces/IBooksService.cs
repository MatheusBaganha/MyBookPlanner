using System;
using System.Collections.Generic;
using System.Text;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels;

namespace MyBookPlanner.Service.Interfaces
{
    public interface IBooksService
    {
        public Task<Result<List<Book>>> GetBooks(int page = 0, int pageSize = 10);
        public Task<Result<Book>> GetBookById(int idBook);
    }    
}
