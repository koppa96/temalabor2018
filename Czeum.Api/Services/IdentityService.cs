using Czeum.Application.Services;
using Czeum.Domain.Services;
using Microsoft.AspNetCore.Http;
using System;

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
            return httpContext.User.Identity.Name ?? throw new InvalidOperationException("Could not identify current user.");
        }
    }
}