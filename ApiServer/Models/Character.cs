using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiServer.Models
{
    public class Character
    {
        [Key]
        public int CharacterId { get; set; }

        [Required]
        [MaxLength(12)]
        public string Name { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}