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

namespace IdentityServer3.EntityFramework
{
    public class OperationalDbContext : BaseDbContext, IOperationalDbContext
    {
        public OperationalDbContext()
            : this(EfConstants.ConnectionName)
        {
        }

        public OperationalDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public OperationalDbContext(string connectionString, string schema)
            : base(connectionString, schema)
        {
        }

        protected override void ConfigureChildCollections()
        {
            this.RegisterConsentChildTablesForDelete<Consent>();
            this.RegisterTokenChildTablesForDelete<Token>();
        }

        public DbSet<Consent> Consents { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureConsents(Schema);
            modelBuilder.ConfigureTokens(Schema);
        }
    }
}
