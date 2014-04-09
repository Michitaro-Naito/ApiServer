namespace ApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayLog_CultureAndTimeZone : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PlayLogs", "CultureCode", c => c.String(nullable: false));
            AlterColumn("dbo.PlayLogs", "TimeZoneCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlayLogs", "TimeZoneCode", c => c.String());
            AlterColumn("dbo.PlayLogs", "CultureCode", c => c.String());
        }
    }
}
