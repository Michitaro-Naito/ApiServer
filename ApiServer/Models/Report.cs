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
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        /// <summary>
        /// When this Report was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// User who made this report.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User's comments which describe what is the problem.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Messages reported.
        /// </summary>
        public string JsonMessages { get; set; }

        [NotMapped]
        public List<MessageInfo> Messages
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<MessageInfo>>(JsonMessages);
                }
                catch
                {
                    return new List<MessageInfo>() { new MessageInfo() { body = JsonMessages } };
                }
            }
        }

        public Report()
        {
            Created = DateTime.UtcNow;
        }
    }
}