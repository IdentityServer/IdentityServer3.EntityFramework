using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Entities
{
    public class Token
    {
        [Key, Column(Order = 0)]
        public virtual string Key { get; set; }

        [Key, Column(Order = 1)]
        public virtual TokenType TokenType { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public virtual string JsonCode { get; set; }

        [Required]
        public virtual DateTime Expiry { get; set; }
    }
}
