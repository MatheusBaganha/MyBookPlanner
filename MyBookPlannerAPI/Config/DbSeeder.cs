using MyBookPlanner.Repository.Data;
using MyBookPlanner.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyBookPlanner.WebApi.Config
{
    public static class DbSeeder
    {
        public static void SeedBooks(MyBookPlannerDataContext context)
        {
            if (context.Books.Any()) return; // If there is books already added in the database, do nothing


            // To make things easier since it's just a side project, all books will have the same image.
            var imageUrl = "https://m.media-amazon.com/images/I/41alKvN9GwL.jpg";

            var books = new List<Book>
            {
                new Book { Title = "No Longer Human", Author = "Osamu Dazai", ReleaseYear = 1948, ImageUrl = imageUrl, Score = 9.0f },
                new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", ReleaseYear = 1937, ImageUrl = imageUrl, Score = 9.5f },
                new Book { Title = "1984", Author = "George Orwell", ReleaseYear = 1949, ImageUrl = imageUrl, Score = 9.0f },
                new Book { Title = "Pride and Prejudice", Author = "Jane Austen", ReleaseYear = 1813, ImageUrl = imageUrl, Score = 8.8f },
                new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", ReleaseYear = 1960, ImageUrl = imageUrl, Score = 9.2f },
                new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ReleaseYear = 1925, ImageUrl = imageUrl, Score = 8.7f },
                new Book { Title = "Brave New World", Author = "Aldous Huxley", ReleaseYear = 1932, ImageUrl = imageUrl, Score = 8.9f },
                new Book { Title = "Crime and Punishment", Author = "Fyodor Dostoevsky", ReleaseYear = 1866, ImageUrl = imageUrl, Score = 9.3f },
                new Book { Title = "The Catcher in the Rye", Author = "J.D. Salinger", ReleaseYear = 1951, ImageUrl = imageUrl, Score = 8.6f },
                new Book { Title = "Moby-Dick", Author = "Herman Melville", ReleaseYear = 1851, ImageUrl = imageUrl, Score = 8.4f },
                new Book { Title = "War and Peace", Author = "Leo Tolstoy", ReleaseYear = 1869, ImageUrl = imageUrl, Score = 9.4f },
                new Book { Title = "The Alchemist", Author = "Paulo Coelho", ReleaseYear = 1988, ImageUrl = imageUrl, Score = 8.6f },
            };

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
