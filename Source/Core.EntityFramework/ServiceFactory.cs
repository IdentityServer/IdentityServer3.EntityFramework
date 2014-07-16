using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.Core.Services.InMemory;

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

        public IClientService CreateClientService()
        {
            return new ClientService(_connectionString);
        }

        public IScopeService CreateScopeService()
        {
            return new ScopeService(_connectionString);
        }

        public IConsentService CreateConsentService()
        {
            return new ConsentService(_connectionString);
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
                var clientsTest = db.Clients.ToList();

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
