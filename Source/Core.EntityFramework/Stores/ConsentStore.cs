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
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class ConsentStore : IConsentStore
    {
        private readonly OperationalDbContext context;

        public ConsentStore(OperationalDbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            
            this.context = context;
        }

        public Task<Models.Consent> LoadAsync(string subject, string client)
        {
            var found = context.Consents.SingleOrDefault(x => x.Subject == subject && x.ClientId == client);
            if (found == null)
            {
                return Task.FromResult<Models.Consent>(null);
            }
                
            var result = new Models.Consent
            {
                Subject = found.Subject,
                ClientId = found.ClientId,
                Scopes = ParseScopes(found.Scopes)
            };

            return Task.FromResult(result);
        }

        public Task UpdateAsync(Models.Consent consent)
        {
            var item = context.Consents.SingleOrDefault(x => x.Subject == consent.Subject && x.ClientId == consent.ClientId);
            if (item == null)
            {
                item = new Entities.Consent { Subject = consent.Subject, ClientId = consent.ClientId };
                context.Consents.Add(item);
            }
                
            if (consent.Scopes == null || !consent.Scopes.Any())
            {
                context.Consents.Remove(item);
            }

            item.Scopes = StringifyScopes(consent.Scopes);

            context.SaveChanges();
            
            return Task.FromResult(0);
        }

        public Task<IEnumerable<Models.Consent>> LoadAllAsync(string subject)
        {
            var found = context.Consents.Where(x => x.Subject == subject).ToArray();
            
            var results = found.Select(x=>new Models.Consent{
                Subject = x.Subject, 
                ClientId = x.ClientId, 
                Scopes = ParseScopes(x.Scopes) 
            });

            return Task.FromResult(results.ToArray().AsEnumerable());
        }

        private IEnumerable<string> ParseScopes(string scopes)
        {
            if (scopes == null || String.IsNullOrWhiteSpace(scopes))
            {
                return Enumerable.Empty<string>();
            }

            return scopes.Split(',');
        }

        private string StringifyScopes(IEnumerable<string> scopes)
        {
            if (scopes == null || !scopes.Any())
            {
                return null;
            }

            return scopes.Aggregate((s1, s2) => s1 + "," + s2);
        }

        public Task RevokeAsync(string subject, string client)
        {
            var found = context.Consents.Where(x => x.Subject == subject && x.ClientId == client);
            
            context.Consents.RemoveRange(found.ToArray());
            
            context.SaveChanges();

            return Task.FromResult(0);
        }
    }
}
