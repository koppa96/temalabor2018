using Microsoft.AspNetCore.Authentication;

namespace Czeum.Tests.IntegrationTests.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationMiddleware>("Bearer", options => { });
        }
    }
}