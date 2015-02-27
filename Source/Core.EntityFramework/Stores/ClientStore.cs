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

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.EntityFramework.Entities;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.EntityFramework
{
    public class ClientStore : IClientStore
    {
        private readonly ClientConfigurationDbContext context;

        public ClientStore(ClientConfigurationDbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            
            this.context = context;
        }

        public async Task<Thinktecture.IdentityServer.Core.Models.Client> FindClientByIdAsync(string clientId)
        {
            var client = await context.Clients
                .Include("ClientSecrets")
                .Include("RedirectUris")
                .Include("PostLogoutRedirectUris")
                .Include("ScopeRestrictions")
                .Include("IdentityProviderRestrictions")
                .Include("Claims")
                .Include("CustomGrantTypeRestrictions")
                .Include("AllowedCorsOrigins")
                .SingleOrDefaultAsync(x => x.ClientId == clientId);

            Thinktecture.IdentityServer.Core.Models.Client model = client.ToModel();
            return model;    
        }
    }
}
