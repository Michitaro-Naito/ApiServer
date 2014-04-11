namespace ApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Report : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ReportId = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        UserId = c.String(),
                        Note = c.String(),
                        JsonMessages = c.String(),
                    })
                .PrimaryKey(t => t.ReportId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reports");
        }
    }
}
