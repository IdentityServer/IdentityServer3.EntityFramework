using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
using Thinktecture.IdentityServer.Core;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.EntityFramework.Serialization;

namespace Thinktecture.IdentityServer.EntityFramework.Tests.Serialization
{
    [TestClass]
    public class ClaimConverterTests
    {
        [TestMethod]
        public void CanSerializeAndDeserializeAClaim()
        {
            var claim = new Claim(Constants.ClaimTypes.Subject, "alice");

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ClaimConverter());
            var json = JsonConvert.SerializeObject(claim, settings);

            claim = JsonConvert.DeserializeObject<Claim>(json, settings);
            Assert.AreEqual(Constants.ClaimTypes.Subject, claim.Type);
            Assert.AreEqual("alice", claim.Value);
        }
    }
}
