using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Extensions;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class ConsentStore : IConsentStore
    {
        private readonly string _connectionString;

        public ConsentStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<bool> RequiresConsentAsync(string client, string subject, IEnumerable<string> scopes)
        {
            var orderedScopes = GetOrderedScopes(scopes);

            using (var db = new CoreDbContext(_connectionString))
            {
                var exists = db.Consents.Any(c => c.ClientId == client &&
                                                c.Scopes == orderedScopes &&
                                                c.Subject == subject);

                return Task.FromResult(!exists);
            }
        }

        private static string GetOrderedScopes(IEnumerable<string> scopes)
        {
            return string.Join(" ", scopes.OrderBy(s => s).ToArray());
        }

        public async Task UpdateConsentAsync(string client, string subject, IEnumerable<string> scopes)
        {
            using (var db = new CoreDbContext(_connectionString))
            {
                var consent = await db.Consents.FindAsync(subject, client);

                if (scopes.Any())
                {
                    if (consent == null)
                    {
                        consent = new Entities.Consent
                        {
                            ClientId = client,
                            Subject = subject,
                        };
                        db.Consents.Add(consent);
                    }

                    consent.Scopes = GetOrderedScopes(scopes);
                }
                else if (consent != null)
                {
                    db.Consents.Remove(consent);
                }

                db.SaveChanges();
            }
        }
    }
}
