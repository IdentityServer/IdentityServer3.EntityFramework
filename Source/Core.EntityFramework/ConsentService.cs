using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class ConsentService : IConsentService
    {
        private readonly string _connectionString;

        public ConsentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<bool> RequiresConsentAsync(Models.Client client, System.Security.Claims.ClaimsPrincipal user, IEnumerable<string> scopes)
        {
            if (!client.RequireConsent)
            {
                return Task.FromResult(false);
            }

            var orderedScopes = string.Join(" ", scopes.OrderBy(s => s).ToArray());

            string subjectId = user.GetSubjectId();

            using (var db = new CoreDbContext(_connectionString))
            {
                var consent = db.Consents.FirstOrDefault(c => c.ClientId == client.ClientId &&
                                                c.Scopes == orderedScopes &&
                                                c.Subject == subjectId);

                return Task.FromResult(consent == null);
            }
        }

        public Task UpdateConsentAsync(Models.Client client, System.Security.Claims.ClaimsPrincipal user, IEnumerable<string> scopes)
        {
            if (client.AllowRememberConsent)
            {
                using (var db = new CoreDbContext(_connectionString))
                {
                    var consent = new Entities.Consent
                    {
                        ClientId = client.ClientId,
                        Subject = user.GetSubjectId(),
                        Scopes = string.Join(" ", scopes.OrderBy(s => s).ToArray())
                    };

                    db.Consents.Add(consent);
                    db.SaveChanges();
                }
            }

            return Task.FromResult(0);
        }
    }
}
