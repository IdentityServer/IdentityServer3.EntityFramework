using System.Security.Claims;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.EntityFramework;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.Core.Services.InMemory;

namespace Thinktecture.IdentityServer.Host.Config
{
    public class EntityFrameworkFactory
    {
        public static IdentityServerServiceFactory Create(
                    string connectionStringName, string issuerUri, string siteName, string publicHostAddress = "")
        {
            var users = new[]
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

            // if we're going to use a database to store our tokens, we'll need to clean up at some point
            ExpiredTokenCollector.Start("name=" + connectionStringName, 5);

            var fact = new IdentityServerServiceFactory
            {
                CoreSettings = Registration.RegisterFactory<CoreSettings>(() => settings),
                UserService = Registration.RegisterFactory<IUserService>(() => userSvc),
                ScopeService = Registration.RegisterFactory<IScopeService>(efServiceFactory.CreateScopeService),
                ClientService = Registration.RegisterFactory<IClientService>(efServiceFactory.CreateClientService),
                ConsentService = Registration.RegisterFactory<IConsentService>(efServiceFactory.CreateConsentService),
                AuthorizationCodeStore = Registration.RegisterFactory<IAuthorizationCodeStore>(efServiceFactory.CreateAuthorizationCodeStore),
                TokenHandleStore = Registration.RegisterFactory<ITokenHandleStore>(efServiceFactory.CreateTokenHandleStore),
                RefreshTokenStore = Registration.RegisterFactory<IRefreshTokenStore>(efServiceFactory.CreateRefreshTokenStore)
            };

            return fact;
        }
    }
}