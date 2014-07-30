using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class ScopeStore : IScopeStore
    {
        private readonly string _connectionString;

        public ScopeStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<IEnumerable<Models.Scope>> GetScopesAsync()
        {
            using (var db = new CoreDbContext(_connectionString))
            {
                var scopes = db.Scopes
                    .Include("ScopeClaims")
                    .ToArray();
                
                var models = scopes.ToList().Select(x => x.ToModel());

                return Task.FromResult(models);
            }
        }
    }
}
