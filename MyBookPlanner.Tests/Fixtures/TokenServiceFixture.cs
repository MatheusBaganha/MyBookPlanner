using Bogus;
using Microsoft.Extensions.Options;
using MyBookPlanner.Domain.Config;
using MyBookPlanner.Domain.Models;
using MyBookPlanner.Service.Services;

namespace MyBookPlanner.Tests.Fixtures
{
    internal class TokenServiceFixture
    {
        private readonly Faker _faker;
        public TokenService Service { get; }

        public TokenServiceFixture()
        {
            _faker = new Faker();

            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "super_secret_key_1234567890_12345678", // 32+ chars
                Issuer = "MyBookPlanner",
                Audience = "MyBookPlannerUsers",
                ExpirationHours = 8
            });

            Service = new TokenService(jwtSettings);
        }

        public TokenService CreateServiceWithShortKey()
        {
            var shortKeySettings = Options.Create(new JwtSettings
            {
                Key = "short_key", // <32 bytes, will fail
                Issuer = "MyBookPlanner",
                Audience = "MyBookPlannerUsers",
                ExpirationHours = 8
            });

            return new TokenService(shortKeySettings);
        }

        public User CreateValidUser()
        {
            return new User
            {
                Id = _faker.Random.Int(1, 1000),
                Username = _faker.Internet.UserName(),
                Email = _faker.Internet.Email(),
                Biography = _faker.Lorem.Sentence(5, 5)
            };
        }
    }
}
