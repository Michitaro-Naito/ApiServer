namespace ApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayLogCreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayLogs", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlayLogs", "Created");
        }
    }
}
