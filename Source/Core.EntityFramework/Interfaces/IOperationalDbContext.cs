using System.Data.Entity;
using IdentityServer3.EntityFramework.Entities;

namespace IdentityServer3.EntityFramework
{
    public interface IOperationalDbContext
    {
        DbSet<Consent> Consents { get; set; }
        DbSet<Token> Tokens { get; set; }
    }
}