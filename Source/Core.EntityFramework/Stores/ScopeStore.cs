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
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.EntityFramework.Entities;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class ScopeStore : IScopeStore
    {
        private readonly ScopeConfigurationDbContext context;

        public ScopeStore(ScopeConfigurationDbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            this.context = context;
        }

        public async Task<IEnumerable<Models.Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            var scopes =
                from s in context.Scopes.Include("ScopeClaims")
                select s;
                
            if (scopeNames != null && scopeNames.Any())
            {
                scopes = from s in scopes
                            where scopeNames.Contains(s.Name)
                            select s;
            }

            var list = await scopes.ToListAsync();
            return list.Select(x => x.ToModel());
        }

        public async Task<IEnumerable<Models.Scope>> GetScopesAsync(bool publicOnly = true)
        {
            var scopes =
                from s in context.Scopes.Include("ScopeClaims")
                select s;
                
            if (publicOnly)
            {
                scopes = from s in scopes
                            where s.ShowInDiscoveryDocument == true
                            select s;
            }

            var list = await scopes.ToListAsync();
            return list.Select(x => x.ToModel());
        }
    }
}
