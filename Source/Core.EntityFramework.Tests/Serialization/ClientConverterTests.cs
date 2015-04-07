using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
using IdentityServer3.Core;
using Newtonsoft.Json;
using IdentityServer3.Core.Models;
using IdentityServer3.EntityFramework.Serialization;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.InMemory;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace IdentityServer3.EntityFramework.Tests.Serialization
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
