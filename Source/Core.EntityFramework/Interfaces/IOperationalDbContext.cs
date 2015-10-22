using System;
using System.Data.Entity;
using System.Threading.Tasks;
using IdentityServer3.EntityFramework.Entities;

namespace IdentityServer3.EntityFramework
{
    public interface IOperationalDbContext : IDisposable
    {
        DbSet<Consent> Consents { get; set; }
        DbSet<Token> Tokens { get; set; }

        Task<int> SaveChangesAsync();
    }
}