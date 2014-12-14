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
using AutoMapper;
using System;
using System.Linq;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public static class Map
    {
        static Map()
        {
            Mapper.CreateMap<string, Uri>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? null : new Uri(s));

            Mapper.CreateMap<Entities.Scope, Models.Scope>(MemberList.Destination)
                .ForMember(x => x.Claims, opts => opts.MapFrom(src => src.ScopeClaims.Select(x => x)));
            Mapper.CreateMap<Entities.ScopeClaim, Models.ScopeClaim>(MemberList.Destination);

            Mapper.CreateMap<Entities.Client, Models.Client>(MemberList.Destination)
                .ForMember(x => x.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => x.Uri)))
                .ForMember(x => x.PostLogoutRedirectUris, opt => opt.MapFrom(src => src.PostLogoutRedirectUris.Select(x => x.Uri)))
                .ForMember(x => x.IdentityProviderRestrictions, opt => opt.MapFrom(src => src.IdentityProviderRestrictions.Select(x => x.Provider)))
                .ForMember(x => x.ScopeRestrictions, opt => opt.MapFrom(src => src.ScopeRestrictions.Select(x => x.Scope)));

            Mapper.CreateMap<Models.Scope, Entities.Scope>(MemberList.Source)
                .ForSourceMember(x => x.Claims, opts => opts.Ignore())
                .ForMember(x => x.ScopeClaims, opts => opts.MapFrom(src => src.Claims.Select(x => x)));
            Mapper.CreateMap<Models.ScopeClaim, Entities.ScopeClaim>(MemberList.Source);

            Mapper.CreateMap<Models.Client, Entities.Client>(MemberList.Source)
                .ForMember(x => x.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => new Entities.ClientRedirectUri { Uri = x })))
                .ForMember(x => x.PostLogoutRedirectUris, opt => opt.MapFrom(src => src.PostLogoutRedirectUris.Select(x => new Entities.PostLogoutRedirectUri { Uri = x })))
                .ForMember(x => x.IdentityProviderRestrictions, opt => opt.MapFrom(src => src.IdentityProviderRestrictions.Select(x => new Entities.IdentityProviderRestriction { Provider = x })))
                .ForMember(x => x.ScopeRestrictions, opt => opt.MapFrom(src => src.ScopeRestrictions.Select(x => new Entities.ClientScopeRestriction { Scope = x })));

            Mapper.AssertConfigurationIsValid();
        }

        public static Models.Scope ToModel(this Entities.Scope s)
        {
            if (s == null) return null;
            return Mapper.Map<Entities.Scope, Models.Scope>(s);
        }

        public static Models.Client ToModel(this Entities.Client s)
        {
            if (s == null) return null;
            return Mapper.Map<Entities.Client, Models.Client>(s);
        }

        public static Entities.Scope ToEntity(this Models.Scope s)
        {
            if (s == null) return null;

            if (s.Claims == null)
            {
                s.Claims = new List<Models.ScopeClaim>();
            }

            return Mapper.Map<Models.Scope, Entities.Scope>(s);
        }

        public static Entities.Client ToEntity(this Models.Client s)
        {
            if (s == null) return null;

            if (s.ScopeRestrictions == null)
            {
                s.ScopeRestrictions = new List<string>();
            }
            if (s.RedirectUris == null)
            {
                s.RedirectUris = new List<string>();
            }
            if (s.PostLogoutRedirectUris == null)
            {
                s.PostLogoutRedirectUris = new List<string>();
            }
            if (s.IdentityProviderRestrictions == null)
            {
                s.IdentityProviderRestrictions = new List<string>();
            }

            return Mapper.Map<Models.Client, Entities.Client>(s);
        }
    }
}
