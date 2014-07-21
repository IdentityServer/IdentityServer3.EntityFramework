using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.Core.EntityFramework.Entities;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class AuthorizationCodeStore : BaseTokenStore<AuthorizationCode>, IAuthorizationCodeStore
    {
        public AuthorizationCodeStore(string connectionString)
            : base(connectionString, TokenType.AuthorizationCode)
        {
        }

        public override Task StoreAsync(string key, AuthorizationCode code)
        {
            using (var db = new CoreDbContext(ConnectionString))
            {
                var efCode = new Entities.Token
                {
                    Key = key,
                    JsonCode = ConvertToJson(code),
                    Expiry = DateTime.UtcNow.AddSeconds(code.Client.AuthorizationCodeLifetime),
                    TokenType = this.tokenType
                };

                db.Tokens.Add(efCode);
                db.SaveChanges();
            }

            return Task.FromResult(0);
        }
    }
}
