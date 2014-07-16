using System.Collections.Generic;
using AutoMapper;
using System;
using System.Linq;
using Thinktecture.IdentityServer.Core.EntityFramework.Entities;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public static class Map
    {
        static Map()
        {
            Mapper.CreateMap<string, Uri>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? null : new Uri(s));

            Mapper.CreateMap<Entities.Scope, Models.Scope>()
                .ForMember(x => x.Claims, opts => opts.MapFrom(src => src.ScopeClaims.Select(x => x)));
            Mapper.CreateMap<Entities.ScopeClaim, Models.ScopeClaim>();

            Mapper.CreateMap<Entities.Client, Models.Client>()
                .ForMember(x => x.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => x.Uri)))
                .ForMember(x => x.ScopeRestrictions, opt => opt.MapFrom(src => src.ScopeRestrictions.Select(x => x.Scope)));

            Mapper.CreateMap<Models.Scope, Entities.Scope>()
                .ForMember(x => x.ScopeClaims, opts => opts.MapFrom(src => src.Claims.Select(x => x)));
            Mapper.CreateMap<Models.ScopeClaim, Entities.ScopeClaim>();

            Mapper.CreateMap<Models.Client, Entities.Client>()
            .ForMember(x => x.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => new Entities.ClientRedirectUri { Uri = x.AbsoluteUri })))
            .ForMember(x => x.ScopeRestrictions, opt => opt.MapFrom(src => src.ScopeRestrictions.Select(x => new Entities.ClientScopeRestriction { Scope = x })));
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
                s.RedirectUris = new List<Uri>();
            }

            return Mapper.Map<Models.Client, Entities.Client>(s);
        }
    }
}
