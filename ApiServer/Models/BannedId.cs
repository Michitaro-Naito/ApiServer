using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiServer.Models
{
    public class BannedId
    {
        [Key]
        public string UserId { get; set; }
    }
}