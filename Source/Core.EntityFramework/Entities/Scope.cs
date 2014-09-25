using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Entities
{
    public class Scope
    {
        [Key]
        public virtual int Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string DisplayName { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Required { get; set; }
        public virtual bool Emphasize { get; set; }
        public virtual int Type { get; set; }

        public virtual ICollection<ScopeClaim> ScopeClaims { get; set; }
    }
}
