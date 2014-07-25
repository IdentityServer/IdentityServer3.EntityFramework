using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.EntityFramework.Serialization;
using Thinktecture.IdentityServer.Core.EntityFramework.Entities;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public abstract class BaseTokenStore<T> where T : class
    {
        private readonly string _connectionString;
        protected readonly TokenType tokenType;

        protected string ConnectionString
        {
            get { return _connectionString; }
        }

        protected BaseTokenStore(string connectionString, TokenType tokenType)
        {
            _connectionString = connectionString;
            this.tokenType = tokenType;
        }

        JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ClaimConverter());
            settings.Converters.Add(new ClaimsPrincipalConverter());
            settings.Converters.Add(new ClientConverter(new ClientService(this.ConnectionString)));
            var svc = new ScopeService(this.ConnectionString);
            var scopes = AsyncHelper.RunSync<IEnumerable<Thinktecture.IdentityServer.Core.Models.Scope>>(async () => await svc.GetScopesAsync());
            settings.Converters.Add(new ScopeConverter(scopes.ToArray()));
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
            using (var db = new CoreDbContext(ConnectionString))
            {
                var token = db.Tokens.FirstOrDefault(c => c.Key == key && c.TokenType == tokenType);
                if (token == null || token.Expiry < DateTime.UtcNow) return Task.FromResult<T>(null);

                T value = ConvertFromJson(token.JsonCode);
                return Task.FromResult(value);
            }
        }

        public Task RemoveAsync(string key)
        {
            using (var db = new CoreDbContext(ConnectionString))
            {
                var code = db.Tokens.FirstOrDefault(c => c.Key == key && c.TokenType == tokenType);

                if (code != null)
                {
                    db.Tokens.Remove(code);
                    db.SaveChanges();
                }
            }

            return Task.FromResult(0);
        }

        public abstract Task StoreAsync(string key, T value);
    }
}
