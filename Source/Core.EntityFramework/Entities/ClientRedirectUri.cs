using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Entities
{
    public class ClientRedirectUri
    {
        [Key]
        public virtual int Id { get; set; }
        [Required]
        public virtual string Uri { get; set; }

        public virtual Client Client { get; set; }
    }
}
