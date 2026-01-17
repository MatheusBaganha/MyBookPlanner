using System.IdentityModel.Tokens.Jwt;
using MyBookPlanner.Tests.Fixtures;

namespace MyBookPlanner.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly TokenServiceFixture _fixture;

        public TokenServiceTests()
        {
            _fixture = new TokenServiceFixture();
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidJwtWithClaims()
        {
            // Arrange
            var user = _fixture.CreateValidUser();

            // Act
            var tokenString = _fixture.Service.GenerateToken(user);

            // Assert
            Assert.False(string.IsNullOrEmpty(tokenString));

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            Assert.NotNull(token);

            // Claims
            var nameClaim = token.Claims.FirstOrDefault(c => c.Type == "unique_name");
            Assert.NotNull(nameClaim);
            Assert.Equal(user.Id.ToString(), nameClaim.Value);

            var emailClaim = token.Claims.FirstOrDefault(c => c.Type == "email");
            Assert.NotNull(emailClaim);
            Assert.Equal(user.Email, emailClaim.Value);

            var usernameClaim = token.Claims.FirstOrDefault(c => c.Type == "username");
            Assert.NotNull(usernameClaim);
            Assert.Equal(user.Username, usernameClaim.Value);

            var bioClaim = token.Claims.FirstOrDefault(c => c.Type == "biography");
            Assert.NotNull(bioClaim);
            Assert.Equal(user.Biography, bioClaim.Value);

            // Expiration ~8 hours
            var expectedExp = DateTime.UtcNow.AddHours(8);
            var actualExp = token.ValidTo;

            // Allow a tolerance of 1 minute for slight timing differences
            Assert.True(Math.Abs((actualExp - expectedExp).TotalMinutes) < 1);
        }

        [Fact]
        public void GenerateToken_ShouldThrowException_WhenKeyTooShort()
        {
            // Arrange
            var serviceWithShortKey = _fixture.CreateServiceWithShortKey();
            var user = _fixture.CreateValidUser();

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => serviceWithShortKey.GenerateToken(user));
        }
    }
}
