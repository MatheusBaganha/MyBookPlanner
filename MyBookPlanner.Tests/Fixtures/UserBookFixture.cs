using Bogus;
using MyBookPlanner.Domain.Constantes;
using MyBookPlanner.Domain.Models;
using MyBookPlannerAPI.ViewModels.UserBooks;

namespace MyBookPlanner.Tests.Fixtures
{
    public class UserBookFixture
    {
        private readonly Faker<UserBook> _userBookFaker;

        public UserBookFixture()
        {
            _userBookFaker = new Faker<UserBook>()
                .RuleFor(ub => ub.IdUser, f => f.Random.Int(1, int.MaxValue))
                .RuleFor(ub => ub.IdBook, f => f.Random.Int(1, int.MaxValue))
                .RuleFor(ub => ub.UserScore, f => f.Random.Float(0, 5))
                .RuleFor(ub => ub.ReadingStatus, f => f.PickRandom(
                    ReadingStatus.Reading,
                    ReadingStatus.Read,
                    ReadingStatus.WishToRead
                ))
                .RuleFor(ub => ub.User, _ => null)
                .RuleFor(ub => ub.Book, _ => null);
        }

        public UserBook CreateValidUserBook()
        {
            return _userBookFaker.Generate();
        }

        public UserBestBookViewModel CreateGenericBestBook(int idUser)
        {
            return new UserBestBookViewModel
            {
                IdUser = idUser,
                IdBook = 0,
                UserScore = 10,
                ReleaseYear = DateTime.UtcNow.Year,
                Title = "MyBookPlannerBook",
                Author = "Author",
                ImageUrl = ""
            };
        }
    }
}
