using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Czeum.Web.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("czeum_api", "Czeum API")
                {
                    UserClaims = { JwtClaimTypes.Name, JwtClaimTypes.Email, JwtClaimTypes.Subject }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "czeum_angular_client",
                    ClientName = "Czeum Offical Angular Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    
                    RedirectUris = { "http://localhost:4200/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:4200/signout-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "czeum_api"
                    },

                    RequirePkce = true,
                    RequireClientSecret = false,
                    RequireConsent = false
                }
            };
        }
    }
}