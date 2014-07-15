using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class AuthorizationCodeStore : BaseTokenStore<AuthorizationCode>, IAuthorizationCodeStore
    {
        public AuthorizationCodeStore(string connectionString)
            : base(connectionString)
        {
        }

        public override Task StoreAsync(string key, AuthorizationCode code)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(ConnectionString))
                {
                    var efCode = new Entities.Token
                    {
                        Key = key,
                        JsonCode = JsonConvert.SerializeObject(code),
                        Expiry = DateTime.UtcNow.AddSeconds(code.Client.AuthorizationCodeLifetime)
                    };

                    db.Tokens.Add(efCode);
                    return db.SaveChanges();
                }
            });
        }
    }
}
