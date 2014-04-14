using ApiScheme.Scheme;
using Newtonsoft.Json;
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

        /*public int WonAsCitizen { get; set; }
        public int WonAsWerewolf { get; set; }
        public int WonAsFox { get; set; }
        public int LostAsCitizen { get; set; }
        public int LostAsWerewolf { get; set; }
        public int LostAsFox { get; set; }*/

        /// <summary>
        /// JSON encoded items like "{foo:2,bar:1}" which means "This Character has 2 foos and 1 bar.".
        /// NOT HABTM for performance reasons.
        /// </summary>
        [Required]
        public string JsonItems { get; set; }

        /// <summary>
        /// A version for optimistic concurrency.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public Character()
        {
            JsonItems = "{}";
        }

        [NotMapped]
        public CharacterItems Items
        {
            get
            {
                return JsonConvert.DeserializeObject<CharacterItems>(JsonItems) ?? new CharacterItems();
            }
            set
            {
                JsonItems = JsonConvert.SerializeObject(value);
            }
        }
    }
}