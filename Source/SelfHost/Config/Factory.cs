using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.EntityFramework;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace SelfHost.Config
{
    class Factory
    {
        public static IdentityServerServiceFactory Configure(string connString)
        {
            var svcFactory = new EntityFrameworkServiceFactory(connString);
            ConfigureClients(Clients.Get(), connString);
            ConfigureScopes(Scopes.Get(), connString);

            var factory = new IdentityServerServiceFactory();
            factory.RegisterConfigurationServices(svcFactory);
            factory.RegisterOperationalServices(svcFactory);

            var userService = new Thinktecture.IdentityServer.Core.Services.InMemory.InMemoryUserService(Users.Get());
            factory.UserService = new Registration<IUserService>(resolver => userService);

            return factory;
        }

        public static void ConfigureClients(IEnumerable<Client> clients, string connString)
        {
            using (var db = new ClientConfigurationDbContext(connString))
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

        public static void ConfigureScopes(IEnumerable<Scope> scopes, string connString)
        {
            using (var db = new ScopeConfigurationDbContext(connString))
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
