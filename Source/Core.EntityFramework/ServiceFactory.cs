using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class ServiceFactory
    {
        private readonly string _connectionString;
        public ServiceFactory(string connectionString)
        {
            _connectionString = connectionString;
            Database.SetInitializer(new CreateDatabaseIfNotExists<CoreDbContext>());
        }

        public IClientStore CreateClientStore()
        {
            return new ClientStore(_connectionString);
        }

        public IScopeStore CreateScopeStore()
        {
            return new ScopeStore(_connectionString);
        }

        public IConsentStore CreateConsentStore()
        {
            return new ConsentStore(_connectionString);
        }

        public IAuthorizationCodeStore CreateAuthorizationCodeStore()
        {
            return new AuthorizationCodeStore(_connectionString);
        }

        public ITokenHandleStore CreateTokenHandleStore()
        {
            return new TokenHandleStore(_connectionString);
        }

        public IRefreshTokenStore CreateRefreshTokenStore()
        {
            return new RefreshTokenStore(_connectionString);
        }

        public void ConfigureClients(IEnumerable<Client> clients)
        {
            using (var db = new CoreDbContext(_connectionString))
            {
                if (!db.Clients.Any())
                {
                    foreach (var c in clients)
                    {
                        var e = c.ToEntity();
                        db.Clients.Add(e);
                    }
                    db.SaveChanges();
                }
            }
        }

        public void ConfigureScopes(IEnumerable<Scope> scopes)
        {
            using (var db = new CoreDbContext(_connectionString))
            {
                if (!db.Scopes.Any())
                {
                    foreach (var s in scopes)
                    {
                        var e = s.ToEntity();
                        db.Scopes.Add(e);
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
