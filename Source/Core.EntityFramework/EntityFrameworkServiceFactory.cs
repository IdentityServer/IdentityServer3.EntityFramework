/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class EntityFrameworkServiceFactory
    {
        private readonly string _connectionString;
        public EntityFrameworkServiceFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Registration<IAuthorizationCodeStore> AuthorizationCodeStore
        {
            get
            {
                return Registration.RegisterFactory<IAuthorizationCodeStore>(() => new AuthorizationCodeStore(_connectionString));
            }
        }

        public Registration<ITokenHandleStore> TokenHandleStore
        {
            get
            {
                return Registration.RegisterFactory<ITokenHandleStore>(() => new TokenHandleStore(_connectionString));
            }
        }

        public Registration<IConsentStore> ConsentStore
        {
            get
            {
                return Registration.RegisterFactory<IConsentStore>(() => new ConsentStore(_connectionString));
            }
        }

        public Registration<IRefreshTokenStore> RefreshTokenStore
        {
            get
            {
                return Registration.RegisterFactory<IRefreshTokenStore>(() => new RefreshTokenStore(_connectionString));
            }
        }

        public Registration<IClientStore> ClientStore
        {
            get
            {
                return Registration.RegisterFactory<IClientStore>(() => new ClientStore(_connectionString));
            }
        }

        public Registration<IScopeStore> ScopeStore
        {
            get
            {
                return Registration.RegisterFactory<IScopeStore>(() => new ScopeStore(_connectionString));
            }
        }
     
        public void ConfigureClients(IEnumerable<Client> clients)
        {
            using (var db = new ConfigurationDbContext(_connectionString))
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
            using (var db = new ConfigurationDbContext(_connectionString))
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
