namespace ApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayLogs",
                c => new
                    {
                        PlayLogId = c.Int(nullable: false, identity: true),
                        RoomName = c.String(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.PlayLogId);

            CreateIndex("dbo.PlayLogs", "FileName", unique: true, name: "IX_RoomName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlayLogs", "IX_RoomName");

            DropTable("dbo.PlayLogs");
        }
    }
}
