using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.EntityFramework.Serialization;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public abstract class BaseTokenStore<T> where T : class
    {
        private readonly string _connectionString;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        protected string ConnectionString
        {
            get { return _connectionString; }
        }

        protected BaseTokenStore(string connectionString)
        {
            _connectionString = connectionString;
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.Converters.Add(new ClaimConverter());
        }

        public Task<T> GetAsync(string key)
        {
            return Task<T>.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(ConnectionString))
                {
                    Entities.Token dbCode = db.Tokens.FirstOrDefault(c => c.Key == key);

                    if (dbCode == null || dbCode.Expiry < DateTime.UtcNow) return null;

                    return JsonConvert.DeserializeObject<T>(dbCode.JsonCode, _jsonSerializerSettings);
                }
            });
        }

        public Task RemoveAsync(string key)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(ConnectionString))
                {
                    var code = db.Tokens.FirstOrDefault(c => c.Key == key);

                    if (code != null)
                    {
                        db.Tokens.Remove(code);
                        db.SaveChanges();
                    }
                }
            });
        }

        public abstract Task StoreAsync(string key, T value);
    }
}
