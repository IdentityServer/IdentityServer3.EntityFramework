using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class RefreshTokenStore : BaseTokenStore<RefreshToken>, IRefreshTokenStore
    {
        public RefreshTokenStore(string connectionstring)
            :base(connectionstring)
        {
        }

        public override Task StoreAsync(string key, RefreshToken value)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var db = new CoreDbContext(ConnectionString))
                {
                    var efToken = new Entities.Token
                    {
                        Key = key,
                        JsonCode = JsonConvert.SerializeObject(value),
                        Expiry = DateTime.UtcNow.AddSeconds(value.LifeTime)
                    };

                    db.Tokens.Add(efToken);
                    db.SaveChanges();
                }
            });
        }
    }
}
