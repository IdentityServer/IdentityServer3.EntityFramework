using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
using Thinktecture.IdentityServer.Core;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.EntityFramework.Serialization;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.Core.Services.InMemory;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace Thinktecture.IdentityServer.v3.EntityFramework.Tests.Serialization
{
   

    [TestClass]
    public class ClientConverterTests
    {
        [TestMethod]
        public void CanSerializeAndDeserializeAClient()
        {
            var client = new Client{
                ClientId = "123", 
                Enabled = true,
                AbsoluteRefreshTokenLifetime = 5, 
                AccessTokenLifetime = 10, 
                AccessTokenType = AccessTokenType.Jwt, 
                AllowRememberConsent = true, 
                RedirectUris = new List<string>{"http://foo.com"}
            };
            var clientStore = new InMemoryClientStore(new Client[]{client});
            var converter = new ClientConverter(clientStore);

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(converter);
            var json = JsonConvert.SerializeObject(client, settings);

            var result = JsonConvert.DeserializeObject<Client>(json, settings);
            Assert.AreSame(client, result);
        }
    }
}
