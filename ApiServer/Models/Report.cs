using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public Report()
        {
            Created = DateTime.UtcNow;
        }
    }
}