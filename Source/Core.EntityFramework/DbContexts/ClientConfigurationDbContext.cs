/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Data.Entity;
using Thinktecture.IdentityServer.EntityFramework.Entities;

namespace Thinktecture.IdentityServer.EntityFramework
{
    public class ClientConfigurationDbContext : BaseDbContext
    {
        public ClientConfigurationDbContext()
            : this(EfConstants.ConnectionName)
        {
        }

        public ClientConfigurationDbContext(string connectionString)
            : base(connectionString)
        {
        }
        
        public ClientConfigurationDbContext(string connectionString, string schema)
            : base(connectionString, schema)
        {
        }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .ToTable(EfConstants.TableNames.Client, Schema);
            modelBuilder.Entity<Client>()
                .HasMany(x => x.ClientSecrets).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.RedirectUris).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.PostLogoutRedirectUris).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.ScopeRestrictions).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.IdentityProviderRestrictions).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.Claims).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.CustomGrantTypeRestrictions).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.AllowedCorsOrigins).WithRequired(x => x.Client).WillCascadeOnDelete();

            modelBuilder.Entity<ClientSecret>().ToTable(EfConstants.TableNames.ClientSecret, Schema);
            modelBuilder.Entity<ClientRedirectUri>().ToTable(EfConstants.TableNames.ClientRedirectUri, Schema);
            modelBuilder.Entity<ClientPostLogoutRedirectUri>().ToTable(EfConstants.TableNames.ClientPostLogoutRedirectUri, Schema);
            modelBuilder.Entity<ClientScopeRestriction>().ToTable(EfConstants.TableNames.ClientScopeRestriction, Schema);
            modelBuilder.Entity<ClientIdPRestriction>().ToTable(EfConstants.TableNames.ClientIdPRestriction, Schema);
            modelBuilder.Entity<ClientClaim>().ToTable(EfConstants.TableNames.ClientClaim, Schema);
            modelBuilder.Entity<ClientGrantTypeRestriction>().ToTable(EfConstants.TableNames.ClientGrantTypeRestriction, Schema);
            modelBuilder.Entity<ClientCorsOrigin>().ToTable(EfConstants.TableNames.ClientCorsOrigin, Schema);
        }
    }
}
