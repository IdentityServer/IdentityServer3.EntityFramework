using System.Security.Claims;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Services.InMemory;

namespace Thinktecture.IdentityServer.Host.Config
{
    public class EntityFrameworkFactory
    {
        public static IdentityServerServiceFactory Create(
                    string connectionStringName, string issuerUri, string siteName, string publicHostAddress = "")
        {
            var users = new []
            {
                new InMemoryUser{Subject = "818727", Username = "alice", Password = "alice", 
                    Claims = new []
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Alice"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Smith"),
                        new Claim(Constants.ClaimTypes.Email, "AliceSmith@email.com")
                    }
                },
                new InMemoryUser{Subject = "88421113", Username = "bob", Password = "bob",
                    Claims = new []
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Bob"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Smith"),
                        new Claim(Constants.ClaimTypes.Email, "BobSmith@email.com")
                    }
                }
            };

            var settings = new LocalTestCoreSettings(issuerUri, siteName, publicHostAddress);

            var userSvc = new InMemoryUserService(users);

            var efServiceFactory = new Core.EntityFramework.ServiceFactory("name=" + connectionStringName);
            efServiceFactory.ConfigureClients(LocalTestClients.Get());
            efServiceFactory.ConfigureScopes(LocalTestScopes.Get());

            var fact = new IdentityServerServiceFactory
            {
                CoreSettings = () => settings,
                UserService = () => userSvc,
                ScopeService = () => efServiceFactory.CreateScopeService(),
                ClientService = () => efServiceFactory.CreateClientService(),
                ConsentService = () => efServiceFactory.CreateConsentService(),
                AuthorizationCodeStore = () => efServiceFactory.CreateAuthorizationCodeStore(5),
                TokenHandleStore = () => efServiceFactory.CreateTokenHandleStore(5)
            };

            return fact;
        }
    }
}