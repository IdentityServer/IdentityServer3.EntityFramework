using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Entities
{
    public class ClientScopeRestriction
    {
        [Key]
        public virtual int Id { get; set; }
        [Required]
        public virtual string Scope { get; set; }

        public virtual Client Client { get; set; }
    }
}
