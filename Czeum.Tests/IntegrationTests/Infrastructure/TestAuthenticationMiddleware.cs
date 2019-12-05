using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Czeum.Domain.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Czeum.Tests.IntegrationTests.Infrastructure
{
    public class TestAuthenticationMiddleware : AuthenticationHandler<TestAuthenticationOptions>
    {
        private readonly UserManager<User> userManager;

        public TestAuthenticationMiddleware(
            IOptionsMonitor<TestAuthenticationOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            UserManager<User> userManager) : base(options, logger, encoder, clock)
        {
            this.userManager = userManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeaderValue = Context.Request.Headers["Authorization"].ToString();
            var user = await userManager.FindByNameAsync(authorizationHeaderValue);

            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim(JwtClaimTypes.Subject, user.Id.ToString())
                }, "Bearer", JwtClaimTypes.Name, JwtClaimTypes.Role));

            return AuthenticateResult.Success(
                    new AuthenticationTicket(claimsPrincipal, "Bearer"));
        }
    }
}