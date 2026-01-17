using Bogus;
using MyBookPlanner.Domain.Models;
using System.Collections.Generic;

namespace MyBookPlanner.Tests.Fixtures
{
    internal class UserFixture
    {
        private readonly Faker<User> _userFaker;

        public UserFixture()
        {
            _userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => f.Random.Int(1, int.MaxValue))
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PasswordHash, f => f.Random.Hash())
                .RuleFor(u => u.Biography, f => f.Lorem.Paragraph())
                .RuleFor(u => u.UserBooks, _ => new List<UserBook>());
        }

        public User CreateValidUser()
        {
            return _userFaker.Generate();
        }

        public List<User> CreateUserList(int quantity = 3)
        {
            return _userFaker.Generate(quantity);
        }
    }
}
