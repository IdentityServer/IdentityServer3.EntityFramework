using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.Connect.Services;
using Thinktecture.IdentityServer.Core.EntityFramework.Serialization;
using System.Timers;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class TokenHandleStore : ITokenHandleStore, ITransientDataRepository<Token>
    {
        private readonly string _connectionString;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public TokenHandleStore(string connectionString, int? cleanupIntervalInMinutes)
        {
            _connectionString = connectionString;
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.Converters.Add(new ClaimConverter());

            if (cleanupIntervalInMinutes.HasValue && cleanupIntervalInMinutes.Value > 0)
            {
                var timer = new Timer
                {
                    AutoReset = true,
                    Interval = cleanupIntervalInMinutes.Value*60*1000
                };
                timer.Elapsed += CleanUpTokens;
                timer.Start();
            }
        }

        private void CleanUpTokens(object sender, ElapsedEventArgs e)
        {
            // Clean up expired tokens
            DateTime referenceDate = DateTime.UtcNow;

            using (var db = new CoreDbContext(_connectionString))
            {
                db.Tokens.RemoveRange(db.Tokens.Where(c => c.Expiry < referenceDate));
                db.SaveChanges();
            }
        }

        public Task<Token> GetAsync(string key)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(_connectionString))
                {
                    Entities.Token dbCode = db.Tokens.FirstOrDefault(c => c.Key == key);

                    if (dbCode == null || dbCode.Expiry < DateTime.UtcNow) return null;

                    return JsonConvert.DeserializeObject<Token>(dbCode.JsonCode, _jsonSerializerSettings);
                }
            });
        }

        public Task RemoveAsync(string key)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(_connectionString))
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

        public Task StoreAsync(string key, Token value)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(_connectionString))
                {
                    var efToken = new Entities.Token
                    {
                        Key = key,
                        JsonCode = JsonConvert.SerializeObject(value),
                        Expiry = DateTime.UtcNow.AddSeconds(value.Lifetime)
                    };

                    db.Tokens.Add(efToken);
                    db.SaveChanges();
                }
            });
        }
    }
}
