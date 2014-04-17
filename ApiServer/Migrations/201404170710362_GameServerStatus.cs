namespace ApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameServerStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameServerStatus",
                c => new
                    {
                        GameServerStatusId = c.Int(nullable: false, identity: true),
                        Updated = c.DateTime(nullable: false),
                        Host = c.String(nullable: false),
                        Port = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        Players = c.Int(nullable: false),
                        MaxPlayers = c.Int(nullable: false),
                        FramesPerInterval = c.Int(nullable: false),
                        ReportIntervalSeconds = c.Double(nullable: false),
                        MaxElapsedSeconds = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.GameServerStatusId);

            CreateIndex("dbo.GameServerStatus", "Name", unique: true, name: "IX_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GameServerStatus", "IX_Name");

            DropTable("dbo.GameServerStatus");
        }
    }
}
