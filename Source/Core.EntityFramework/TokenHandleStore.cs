using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class TokenHandleStore : BaseTokenStore<Token>, ITokenHandleStore
    {

        public TokenHandleStore(string connectionString)
            : base(connectionString)
        {
        }

        public override Task StoreAsync(string key, Token value)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(ConnectionString))
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
