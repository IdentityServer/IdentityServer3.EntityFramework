using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Entities
{
    public class ScopeClaim
    {
        [Key]
        public virtual int Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool AlwaysIncludeInIdToken { get; set; }

        public virtual Scope Scope { get; set; }
    }
}
