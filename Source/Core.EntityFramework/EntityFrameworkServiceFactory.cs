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
using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class EntityFrameworkServiceFactory
    {
        private readonly string _connectionString;
        public EntityFrameworkServiceFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Registration<IAuthorizationCodeStore> AuthorizationCodeStore
        {
            get
            {
                return new Registration<IAuthorizationCodeStore>(resolver => new AuthorizationCodeStore(_connectionString, resolver.Resolve<IScopeStore>(), resolver.Resolve<IClientStore>()));
            }
        }

        public Registration<ITokenHandleStore> TokenHandleStore
        {
            get
            {
                return new Registration<ITokenHandleStore>(resolver => new TokenHandleStore(_connectionString, resolver.Resolve<IScopeStore>(), resolver.Resolve<IClientStore>()));
            }
        }

        public Registration<IConsentStore> ConsentStore
        {
            get
            {
                return new Registration<IConsentStore>(resolver => new ConsentStore(_connectionString));
            }
        }

        public Registration<IRefreshTokenStore> RefreshTokenStore
        {
            get
            {
                return new Registration<IRefreshTokenStore>(resolver => new RefreshTokenStore(_connectionString, resolver.Resolve<IScopeStore>(), resolver.Resolve<IClientStore>()));
            }
        }

        public Registration<IClientStore> ClientStore
        {
            get
            {
                return new Registration<IClientStore>(resolver => new ClientStore(_connectionString));
            }
        }

        public Registration<IScopeStore> ScopeStore
        {
            get
            {
                return new Registration<IScopeStore>(resolver => new ScopeStore(_connectionString));
            }
        }
    }
}