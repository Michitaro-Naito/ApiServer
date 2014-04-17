using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiServer.Models
{
    public class GameServerStatus
    {
        /// <summary>
        /// A surrogate identifier.
        /// </summary>
        [Key]
        public int GameServerStatusId { get; set; }

        /// <summary>
        /// UTC when updated.
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Hostname like "111.111.111.111", "server1.example.com"
        /// </summary>
        [Required]
        public string Host { get; set; }

        /// <summary>
        /// Port like "8080", "80".
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Name like "東京第一サーバ".
        /// UNIQUE
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Current connected players.
        /// </summary>
        public int Players { get; set; }

        /// <summary>
        /// A limit of the number of players.
        /// </summary>
        public int MaxPlayers { get; set; }

        /// <summary>
        /// Frames per interval like "5000".
        /// Ideal GameServer has: FramesPerInterval / ReportIntervalSeconds > 100.
        /// Empty GameServer maybe have: > 500.
        /// </summary>
        public int FramesPerInterval { get; set; }

        /// <summary>
        /// GameServer reports it's status every this seconds like "60", "300".
        /// </summary>
        public double ReportIntervalSeconds { get; set; }

        /// <summary>
        /// Max elapsed seconds. If bigger, it's lagger.
        /// Ideal GameServer has : < 0.01.
        /// </summary>
        public double MaxElapsedSeconds { get; set; }
    }
}