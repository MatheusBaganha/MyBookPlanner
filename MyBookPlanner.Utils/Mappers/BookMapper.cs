using System;
using System.Collections.Generic;
using System.Text;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels.Books;
using MyBookPlanner.Domain.ViewModels.Users;

namespace MyBookPlanner.Utils.Mappers
{
    public static class BookMapper
    {
        public static BookViewModel Convert(this Book book)
        {
            return new BookViewModel()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ImageUrl = book.ImageUrl,
                ReleaseYear = book.ReleaseYear,
                Score = book.Score  
            };
        }

        public static List<BookViewModel> ConvertList(this List<Book> books)
        {
            return books.Select(x => x.Convert()).ToList();
        }
    }
}