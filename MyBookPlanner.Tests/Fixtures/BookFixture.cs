using Bogus;
using MyBookPlanner.Domain.Models;

namespace MyBookPlanner.Tests.Fixtures
{
    internal class BookFixture
    {
        private readonly Faker<Book> _bookFaker;

        public BookFixture()
        {
            _bookFaker = new Faker<Book>()
                .RuleFor(b => b.Id, f => f.Random.Int(1, int.MaxValue))
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
                .RuleFor(b => b.Author, f => f.Person.FullName)
                .RuleFor(b => b.ReleaseYear, f => f.Date.Past(50).Year)
                .RuleFor(b => b.ImageUrl, f => f.Internet.Url())
                .RuleFor(b => b.Score, f => (float)Math.Round(f.Random.Float(1, 10), 1))
                .RuleFor(b => b.UserBooks, _ => new List<UserBook>());
        }

        public Book CreateValidBook()
        {
            return _bookFaker.Generate();
        }

        public List<Book> CreateBookList(int quantity = 3)
        {
            return _bookFaker.Generate(quantity);
        }
    }
}
