using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.Connect.Services;
using Thinktecture.IdentityServer.Core.EntityFramework.Serialization;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class AuthorizationCodeStore : IAuthorizationCodeStore, ITransientDataRepository<AuthorizationCode>
    {
        private readonly string _connectionString;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public AuthorizationCodeStore(string connectionString, int? cleanupIntervalInMinutes)
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

        public Task<AuthorizationCode> GetAsync(string key)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(_connectionString))
                {
                    Entities.AuthorizationCode dbCode = db.AuthorizationCodes.FirstOrDefault(c => c.Key == key);

                    if (dbCode == null || dbCode.Expiry < DateTime.UtcNow) return null;

                    return JsonConvert.DeserializeObject<AuthorizationCode>(dbCode.JsonCode, _jsonSerializerSettings);
                }
            });
        }

        public Task RemoveAsync(string key)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(_connectionString))
                {
                    var code = db.AuthorizationCodes.FirstOrDefault(c => c.Key == key);

                    if (code != null)
                    {
                        db.AuthorizationCodes.Remove(code);
                        db.SaveChanges();
                    }
                }
            });
        }

        public Task StoreAsync(string key, AuthorizationCode code)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(_connectionString))
                {
                    var efCode = new Entities.AuthorizationCode
                    {
                        Key = key,
                        JsonCode = JsonConvert.SerializeObject(code),
                        Expiry = DateTime.UtcNow.AddSeconds(code.Client.AuthorizationCodeLifetime)
                    };

                    db.AuthorizationCodes.Add(efCode);
                    return db.SaveChanges();
                }
            });
        }
    }
}
