using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.Core.EntityFramework.Entities;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class RefreshTokenStore : BaseTokenStore<RefreshToken>, IRefreshTokenStore
    {
        public RefreshTokenStore(string connectionstring)
            : base(connectionstring, TokenType.RefreshToken)
        {
        }

        public override Task StoreAsync(string key, RefreshToken value)
        {
            using (var db = new CoreDbContext(ConnectionString))
            {
                var efToken = new Entities.Token
                {
                    Key = key,
                    JsonCode = ConvertToJson(value),
                    Expiry = DateTime.UtcNow.AddSeconds(value.LifeTime),
                    TokenType = this.tokenType
                };

                db.Tokens.Add(efToken);
                db.SaveChanges();
            }

            return Task.FromResult(0);
        }
    }
}
