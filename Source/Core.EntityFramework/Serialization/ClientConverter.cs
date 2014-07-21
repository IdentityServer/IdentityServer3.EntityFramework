using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        IClientService clientService;
        public ClientConverter(IClientService clientService)
        {
            if (clientService == null) throw new ArgumentNullException("clientService");

            this.clientService = clientService;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Client) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ClientLite>(reader);
            return AsyncHelper.RunSync<Client>(async () => await clientService.FindClientByIdAsync(source.ClientId));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Client source = (Client)value;

            var target = new ClientLite
            {
                ClientId = source.ClientId
            };
            serializer.Serialize(writer, target);
        }
    }
}
