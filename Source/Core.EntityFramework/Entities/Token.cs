using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Entities
{
    public class Token
    {
        [Key]
        public virtual string Key { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public virtual string JsonCode { get; set; }

        [Required]
        public virtual DateTime Expiry { get; set; }

        [Required]
        public virtual TokenType TokenType { get; set; }
    }
}
