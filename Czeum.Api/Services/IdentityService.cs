using Czeum.Application.Services;
using Microsoft.AspNetCore.Http;

namespace Czeum.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpContext httpContext;
        
        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        
        public string GetCurrentUser()
        {
            return httpContext.User.Identity.Name;
        }
    }
}