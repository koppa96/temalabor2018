using System;
using Czeum.Domain.Services;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;

namespace Czeum.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpContext httpContext;
        
        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        
        public string GetCurrentUserName()
        {
            return httpContext.User.Identity.Name ?? throw new InvalidOperationException("Could not identify current user.");
        }

        public Guid GetCurrentUserId()
        {
            return Guid.Parse(httpContext.User.GetSubjectId());
        }
    }
}