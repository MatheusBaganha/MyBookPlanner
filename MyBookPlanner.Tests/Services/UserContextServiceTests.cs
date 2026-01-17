using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using MyBookPlanner.Service.Services;
using Xunit;

namespace MyBookPlanner.Tests.Services
{
    public class UserContextServiceTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly UserContextService _service;

        public UserContextServiceTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _service = new UserContextService(_httpContextAccessorMock.Object);
        }

        [Fact]
        public void GetLoggedUserId_ShouldReturnUserId_WhenClaimExists()
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.Name, "123") };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = user };

            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(context);

            // Act
            var result = _service.GetLoggedUserId();

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public void GetLoggedUserId_ShouldReturnZero_WhenNoHttpContext()
        {
            // Arrange
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns((HttpContext)null);

            // Act
            var result = _service.GetLoggedUserId();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetLoggedUserId_ShouldReturnZero_WhenNoUser()
        {
            // Arrange
            var context = new DefaultHttpContext { User = null };
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(context);

            // Act
            var result = _service.GetLoggedUserId();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetLoggedUserId_ShouldReturnZero_WhenClaimIsNotInt()
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.Name, "abc") };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = user };
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(context);

            // Act
            var result = _service.GetLoggedUserId();

            // Assert
            Assert.Equal(0, result);
        }
    }
}
