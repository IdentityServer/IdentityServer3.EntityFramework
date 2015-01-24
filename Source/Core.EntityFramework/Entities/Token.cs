/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thinktecture.IdentityServer.EntityFramework.Entities
{
    public class Token
    {
        [Key, Column(Order = 0)]
        public virtual string Key { get; set; }

        [Key, Column(Order = 1)]
        public virtual TokenType TokenType { get; set; }

        [StringLength(200)]
        public virtual string SubjectId { get; set; }
        
        [Required]
        [StringLength(200)]
        public virtual string ClientId { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        public virtual string JsonCode { get; set; }

        [Required]
        public virtual DateTimeOffset Expiry { get; set; }
    }
}
