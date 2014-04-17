using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ApiServer.Models
{
    /*// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }*/

    public class MyDbContext : DbContext
    {
        public MyDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<PlayLog> PlayLogs { get; set; }
        public DbSet<BannedId> BannedIds { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<GameServerStatus> GameServerStatuses { get; set; }
    }
}