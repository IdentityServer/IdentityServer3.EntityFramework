using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
using Thinktecture.IdentityServer.Core;
using Newtonsoft.Json;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.EntityFramework.Serialization;

namespace Thinktecture.IdentityServer.v3.EntityFramework.Tests.Serialization
{
    [TestClass]
    public class ClaimsPrincipalConverterTests
    {
        [TestMethod]
        public void CanSerializeAndDeserializeAClaimsPrincipal()
        {
            var claims = new Claim[]{
                new Claim(Constants.ClaimTypes.Subject, "alice"),
                new Claim(Constants.ClaimTypes.Scope, "read"),
                new Claim(Constants.ClaimTypes.Scope, "write"),
            };
            var ci = new ClaimsIdentity(claims, Constants.AuthenticationMethods.Password);
            var cp = new ClaimsPrincipal(ci);

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ClaimsPrincipalConverter());
            var json = JsonConvert.SerializeObject(cp, settings);

            cp = JsonConvert.DeserializeObject<ClaimsPrincipal>(json, settings);
            Assert.AreEqual(Constants.AuthenticationMethods.Password, cp.Identity.AuthenticationType);
            Assert.AreEqual(3, cp.Claims.Count());
            Assert.IsTrue(cp.HasClaim(Constants.ClaimTypes.Subject, "alice"));
            Assert.IsTrue(cp.HasClaim(Constants.ClaimTypes.Scope, "read"));
            Assert.IsTrue(cp.HasClaim(Constants.ClaimTypes.Scope, "write"));
        }
    }
}
