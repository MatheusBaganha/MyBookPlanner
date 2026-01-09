using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MyBookPlanner.Service.Interfaces;

namespace MyBookPlanner.Service.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetLoggedUserId()
        {
            // Get the claim value
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

            // Try parse safely
            return int.TryParse(userIdStr, out var id) ? id : 0;
        }
    }

}
