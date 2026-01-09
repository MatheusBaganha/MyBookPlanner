using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MyBookPlanner.WebApi.Config.Requirements;

namespace MyBookPlanner.WebApi.Config.HandlerRequirements
{
    public class SameUserHandler : AuthorizationHandler<SameUserRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SameUserHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SameUserRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // Gets the logged user id
            var userIdStr = context.User.FindFirstValue(ClaimTypes.Name);
            if (!int.TryParse(userIdStr, out var loggedUserId))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // takes the idUser from the params or from the body
            int targetUserId = 0;

            if (httpContext.Request.RouteValues.TryGetValue("idUser", out var routeId))
                int.TryParse(routeId?.ToString(), out targetUserId);
            else if (httpContext.Request.Query.TryGetValue("idUser", out var queryId))
                int.TryParse(queryId, out targetUserId);

            // check if the user is doing the operation in his own account.
            if (targetUserId != loggedUserId)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
