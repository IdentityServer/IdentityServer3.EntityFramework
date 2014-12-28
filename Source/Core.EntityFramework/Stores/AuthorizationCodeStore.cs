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
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.EntityFramework.Entities;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class AuthorizationCodeStore : BaseTokenStore<AuthorizationCode>, IAuthorizationCodeStore
    {
        public AuthorizationCodeStore(string connectionString, IScopeStore scopeStore, IClientStore clientStore)
            : base(connectionString, TokenType.AuthorizationCode, scopeStore, clientStore)
        {
        }

        public override Task StoreAsync(string key, AuthorizationCode code)
        {
            using (var db = new OperationalDbContext(ConnectionString))
            {
                var efCode = new Entities.Token
                {
                    Key = key,
                    SubjectId = code.SubjectId,
                    ClientId = code.ClientId,
                    JsonCode = ConvertToJson(code),
                    Expiry = DateTimeOffset.UtcNow.AddSeconds(code.Client.AuthorizationCodeLifetime),
                    TokenType = this.TokenType
                };

                db.Tokens.Add(efCode);
                db.SaveChanges();
            }

            return Task.FromResult(0);
        }
    }
}
