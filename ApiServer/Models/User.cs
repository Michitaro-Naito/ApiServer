﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiServer.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }

        public string IpAddress { get; set; }
        public string Host { get; set; }
        public DateTime Created { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Character> Characters { get; set; }

        public User()
        {
            Characters = new List<Character>();
        }
    }
}