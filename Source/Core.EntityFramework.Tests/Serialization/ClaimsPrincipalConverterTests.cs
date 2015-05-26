using System;
using System.Linq;
using System.Security.Claims;
using IdentityServer3.Core;
using Newtonsoft.Json;
using IdentityServer3.Core.Models;
using IdentityServer3.EntityFramework.Serialization;
using Xunit;

namespace IdentityServer3.EntityFramework.Tests.Serialization
{
    public class ClaimsPrincipalConverterTests
    {
        [Fact]
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
            Assert.Equal(Constants.AuthenticationMethods.Password, cp.Identity.AuthenticationType);
            Assert.Equal(3, cp.Claims.Count());
            Assert.True(cp.HasClaim(Constants.ClaimTypes.Subject, "alice"));
            Assert.True(cp.HasClaim(Constants.ClaimTypes.Scope, "read"));
            Assert.True(cp.HasClaim(Constants.ClaimTypes.Scope, "write"));
        }
    }
}
