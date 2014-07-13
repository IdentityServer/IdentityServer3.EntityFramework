using System;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Serialization
{
    public class ClaimConverter : JsonCreationConverter<Claim>
    {
        protected override Claim Create(Type objectType, JObject jObject)
        {
            string issuer = FieldExists("Issuer", jObject) ? jObject["Issuer"].Value<string>() : null;
            string originalIssuer = FieldExists("OriginalIssuer", jObject) ? jObject["OriginalIssuer"].Value<string>() : null;
            ClaimsIdentity subject = FieldExists("Subject", jObject) ? jObject["Subject"].Value<ClaimsIdentity>() : null;
            string type = FieldExists("Type", jObject) ? jObject["Type"].Value<string>() : null;
            string value = FieldExists("Value", jObject) ? jObject["Value"].Value<string>() : null;
            string valueType = FieldExists("valueType", jObject) ? jObject["valueType"].Value<string>() : null;

            return new Claim(type, value, valueType, issuer, originalIssuer, subject);
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }
}
