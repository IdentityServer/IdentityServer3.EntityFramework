using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thinktecture.IdentityServer.Core.EntityFramework.Entities
{
    public class Token
    {
        [Key]
        public string Key { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string JsonCode { get; set; }

        [Required]
        public DateTime Expiry { get; set; }

        [Required]
        public TokenType TokenType { get; set; }
    }
}
