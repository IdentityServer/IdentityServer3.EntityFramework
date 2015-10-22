using System.Data.Entity;
using IdentityServer3.EntityFramework.Entities;

namespace IdentityServer3.EntityFramework
{
    public interface IScopeConfigurationDbContext
    {
        DbSet<Scope> Scopes { get; set; }
    }
}