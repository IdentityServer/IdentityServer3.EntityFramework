using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class ClientStore : IClientStore
    {
        private readonly string _connectionString;

        public ClientStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<Models.Client> FindClientByIdAsync(string clientId)
        {
            using(var db = new CoreDbContext(_connectionString))
            {
                var client = db.Clients
                    .Include("RedirectUris")
                    .Include("ScopeRestrictions")
                    .SingleOrDefault(x => x.ClientId == clientId);

                Models.Client model = client.ToModel();
                return Task.FromResult(model);    
            }
        }
    }
}
