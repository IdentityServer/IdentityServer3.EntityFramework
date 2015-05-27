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
using IdentityServer3.EntityFramework.Entities;
using System.Collections.Specialized;

namespace IdentityServer3.EntityFramework
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

        protected override void ConfigureChildCollections()
        {
            this.Set<Client>().Local.CollectionChanged +=
                delegate(object sender, NotifyCollectionChangedEventArgs e)
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (Client item in e.NewItems)
                        {
                            RegisterDeleteOnRemove(item.ClientSecrets);
                            RegisterDeleteOnRemove(item.RedirectUris);
                            RegisterDeleteOnRemove(item.PostLogoutRedirectUris);
                            RegisterDeleteOnRemove(item.AllowedScopes);
                            RegisterDeleteOnRemove(item.IdentityProviderRestrictions);
                            RegisterDeleteOnRemove(item.Claims);
                            RegisterDeleteOnRemove(item.AllowedCustomGrantTypes);
                            RegisterDeleteOnRemove(item.AllowedCorsOrigins);
                        }
                    }
                };
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
                .HasMany(x => x.AllowedScopes).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.IdentityProviderRestrictions).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.Claims).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.AllowedCustomGrantTypes).WithRequired(x => x.Client).WillCascadeOnDelete();
            modelBuilder.Entity<Client>()
                .HasMany(x => x.AllowedCorsOrigins).WithRequired(x => x.Client).WillCascadeOnDelete();

            modelBuilder.Entity<ClientSecret>().ToTable(EfConstants.TableNames.ClientSecret, Schema);
            modelBuilder.Entity<ClientRedirectUri>().ToTable(EfConstants.TableNames.ClientRedirectUri, Schema);
            modelBuilder.Entity<ClientPostLogoutRedirectUri>().ToTable(EfConstants.TableNames.ClientPostLogoutRedirectUri, Schema);
            modelBuilder.Entity<ClientScope>().ToTable(EfConstants.TableNames.ClientScopes, Schema);
            modelBuilder.Entity<ClientIdPRestriction>().ToTable(EfConstants.TableNames.ClientIdPRestriction, Schema);
            modelBuilder.Entity<ClientClaim>().ToTable(EfConstants.TableNames.ClientClaim, Schema);
            modelBuilder.Entity<ClientCustomGrantType>().ToTable(EfConstants.TableNames.ClientCustomGrantType, Schema);
            modelBuilder.Entity<ClientCorsOrigin>().ToTable(EfConstants.TableNames.ClientCorsOrigin, Schema);
        }
    }
}
