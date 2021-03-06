﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ApiServer.Models
{
    /// <summary>
    /// Information about a PlayLog.
    /// (PlayLog itself is stored in Azure Blob Storage.)
    /// </summary>
    public class PlayLog
    {
        /// <summary>
        /// A surrogate key.
        /// </summary>
        [Key]
        public int PlayLogId { get; set; }

        /// <summary>
        /// UTC when it was created.
        /// </summary>
        public DateTime Created { get; set; }

        [Required]
        public string CultureCode { get; set; }

        [Required]
        public string TimeZoneCode { get; set; }

        /// <summary>
        /// A RoomName like "Come to Play!", "みんなで楽しく人狼ゲーム！".
        /// </summary>
        [Required]
        public string RoomName { get; set; }

        /// <summary>
        /// An unique FileName like "2014-04-08_08-07-11_5569572.html".
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        public PlayLog()
        {
            Created = DateTime.UtcNow;
            CultureCode = "en-US";
            TimeZoneCode = "UTC";
        }

        public static PlayLog FromInfo(ApiScheme.Scheme.PlayLogInfo info)
        {
            return new PlayLog()
            {
                CultureCode = info.culture,
                TimeZoneCode = info.timezone,
                RoomName = info.roomName,
                FileName = info.fileName
            };
        }

        public ApiScheme.Scheme.PlayLogInfo ToInfo()
        {
            return new ApiScheme.Scheme.PlayLogInfo()
            {
                id = PlayLogId,
                created = Created,
                culture = CultureCode,
                timezone = TimeZoneCode,
                roomName = RoomName,
                fileName = FileName
            };
        }
    }
}