using Newtonsoft.Json;
using System;
using System.Linq;
using Thinktecture.IdentityServer.Core.Models;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Serialization
{
    public class ScopeLite
    {
        public string Name { get; set; }
    }

    public class ScopeConverter : JsonConverter
    {
        private readonly Scope[] _scopes;

        public ScopeConverter(Scope[] scopes)
        {
            if (scopes == null) throw new ArgumentNullException("scopes");

            _scopes = scopes;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Scope) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ScopeLite>(reader);
            return _scopes.Single(x => x.Name == source.Name);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (Scope)value;

            var target = new ScopeLite
            {
                Name = source.Name
            };
            serializer.Serialize(writer, target);
        }
    }
}
