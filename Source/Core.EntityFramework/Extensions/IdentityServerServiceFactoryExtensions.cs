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

using System;
using IdentityServer3.EntityFramework;
using IdentityServer3.Core.Services;

namespace IdentityServer3.Core.Configuration
{
    public static class IdentityServerServiceFactoryExtensions
    {
        public static void RegisterOperationalServices(this IdentityServerServiceFactory factory, EntityFrameworkServiceOptions options)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            if (options == null) throw new ArgumentNullException("options");

            if (options.SynchronousReads)
            {
                factory.Register(new Registration<EntityFrameworkServiceOptions>(options));
            }

            factory.Register(new Registration<IOperationalDbContext>(resolver => new OperationalDbContext(options.ConnectionString, options.Schema)));
            factory.AuthorizationCodeStore = new Registration<IAuthorizationCodeStore, AuthorizationCodeStore>();
            factory.TokenHandleStore = new Registration<ITokenHandleStore, TokenHandleStore>();
            factory.ConsentStore = new Registration<IConsentStore, ConsentStore>();
            factory.RefreshTokenStore = new Registration<IRefreshTokenStore, RefreshTokenStore>();
        }

        public static void RegisterConfigurationServices(this IdentityServerServiceFactory factory, EntityFrameworkServiceOptions options)
        {
            factory.RegisterClientStore(options);
            factory.RegisterScopeStore(options);
        }

        public static void RegisterClientStore(this IdentityServerServiceFactory factory, EntityFrameworkServiceOptions options)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            if (options == null) throw new ArgumentNullException("options");

            if (options.SynchronousReads)
            {
                factory.Register(new Registration<EntityFrameworkServiceOptions>(options));
            }

            factory.Register(new Registration<IClientConfigurationDbContext>(resolver => new ClientConfigurationDbContext(options.ConnectionString, options.Schema)));
            factory.ClientStore = new Registration<IClientStore, ClientStore>();
            factory.CorsPolicyService = new ClientConfigurationCorsPolicyRegistration(options);
        }
        
        public static void RegisterScopeStore(this IdentityServerServiceFactory factory, EntityFrameworkServiceOptions options)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            if (options == null) throw new ArgumentNullException("options");

            if (options.SynchronousReads)
            {
                factory.Register(new Registration<EntityFrameworkServiceOptions>(options));
            }

            factory.Register(new Registration<IScopeConfigurationDbContext>(resolver => new ScopeConfigurationDbContext(options.ConnectionString, options.Schema)));
            factory.ScopeStore = new Registration<IScopeStore, ScopeStore>();
        }
    }
}
