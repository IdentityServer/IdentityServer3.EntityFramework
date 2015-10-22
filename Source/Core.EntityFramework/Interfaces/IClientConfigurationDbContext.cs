using System.Data.Entity;
using IdentityServer3.EntityFramework.Entities;

namespace IdentityServer3.EntityFramework
{
    public interface IClientConfigurationDbContext
    {
        DbSet<Client> Clients { get; set; }
    }
}