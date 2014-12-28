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
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.EntityFramework.Serialization;
using Thinktecture.IdentityServer.Core.EntityFramework.Entities;
using System.Collections.Generic;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public abstract class BaseTokenStore<T> where T : class
    {
        private readonly string _connectionString;
        protected readonly TokenType TokenType;
        protected readonly IScopeStore _scopeStore;
        protected readonly IClientStore _clientStore;

        protected string ConnectionString
        {
            get { return _connectionString; }
        }

        protected BaseTokenStore(string connectionString, TokenType tokenType, IScopeStore scopeStore, IClientStore clientStore)
        {
            _connectionString = connectionString;
            TokenType = tokenType;
            _scopeStore = scopeStore;
            _clientStore = clientStore;
        }

        JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ClaimConverter());
            settings.Converters.Add(new ClaimsPrincipalConverter());
            settings.Converters.Add(new ClientConverter(_clientStore));
            settings.Converters.Add(new ScopeConverter(_scopeStore));
            return settings;
        }

        protected string ConvertToJson(T value)
        {
            return JsonConvert.SerializeObject(value, GetJsonSerializerSettings());
        }

        protected T ConvertFromJson(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, GetJsonSerializerSettings());
        }

        public Task<T> GetAsync(string key)
        {
            using (var db = new OperationalDbContext(ConnectionString))
            {
                var token = db.Tokens.FirstOrDefault(c => c.Key == key && c.TokenType == TokenType);
                if (token == null || token.Expiry < DateTimeOffset.UtcNow) return Task.FromResult<T>(null);

                T value = ConvertFromJson(token.JsonCode);
                return Task.FromResult(value);
            }
        }

        public Task RemoveAsync(string key)
        {
            using (var db = new OperationalDbContext(ConnectionString))
            {
                var code = db.Tokens.FirstOrDefault(c => c.Key == key && c.TokenType == TokenType);

                if (code != null)
                {
                    db.Tokens.Remove(code);
                    db.SaveChanges();
                }
            }

            return Task.FromResult(0);
        }

        public Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
        {
            using (var db = new OperationalDbContext(ConnectionString))
            {
                var tokens = db.Tokens.Where(x => 
                    x.SubjectId == subject &&
                    x.TokenType == TokenType).ToArray();
                var results = tokens.Select(x=>ConvertFromJson(x.JsonCode)).ToArray();
                
                return Task.FromResult(results.Cast<ITokenMetadata>());
            }
        }
        
        public Task RevokeAsync(string subject, string client)
        {
            using (var db = new OperationalDbContext(ConnectionString))
            {
                var found = db.Tokens.Where(x => 
                    x.SubjectId == subject && 
                    x.ClientId == client && 
                    x.TokenType == TokenType).ToArray();
                db.Tokens.RemoveRange(found);

                db.SaveChanges();
            }

            return Task.FromResult(0);
        }

        public abstract Task StoreAsync(string key, T value);
    }
}
