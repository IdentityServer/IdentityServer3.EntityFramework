using Newtonsoft.Json;
using System;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Serialization
{
    public class ClientLite
    {
        public string ClientId { get; set; }
    }

    public class ClientConverter : JsonConverter
    {
        private readonly IClientStore _clientStore;

        public ClientConverter(IClientStore clientStore)
        {
            if (clientStore == null) throw new ArgumentNullException("clientStore");

            _clientStore = clientStore;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Client) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ClientLite>(reader);
            return AsyncHelper.RunSync(async () => await _clientStore.FindClientByIdAsync(source.ClientId));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (Client)value;

            var target = new ClientLite
            {
                ClientId = source.ClientId
            };
            serializer.Serialize(writer, target);
        }
    }
}
