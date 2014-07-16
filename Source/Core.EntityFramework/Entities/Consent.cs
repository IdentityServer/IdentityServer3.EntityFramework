using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Entities
{
    public class Consent
    {
        [Key,Column(Order=0)]
        public virtual string Subject { get; set; }

        [Key, Column(Order = 1)]
        public virtual string ClientId { get; set; }

        [Required]
        public virtual string Scopes { get; set; }
    }
}
