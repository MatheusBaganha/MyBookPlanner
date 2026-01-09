using Microsoft.AspNetCore.Authorization;

namespace MyBookPlanner.WebApi.Config.Requirements
{
    public class SameUserRequirement : IAuthorizationRequirement
    {
        public SameUserRequirement() { }
    }
}
