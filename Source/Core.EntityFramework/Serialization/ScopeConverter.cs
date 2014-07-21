using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Serialization
{
    public class ScopeLite
    {
        public string Name { get; set; }
    }

    public class ScopeConverter : JsonConverter
    {
        Scope[] scopes;
        public ScopeConverter(Scope[] scopes)
        {
            if (scopes == null) throw new ArgumentNullException("scopes");

            this.scopes = scopes;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Scope) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ScopeLite>(reader);
            return scopes.Single(x => x.Name == source.Name);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Scope source = (Scope)value;

            var target = new ScopeLite
            {
                Name = source.Name
            };
            serializer.Serialize(writer, target);
        }
    }
}
