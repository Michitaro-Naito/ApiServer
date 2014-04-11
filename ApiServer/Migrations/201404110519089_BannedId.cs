namespace ApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BannedId : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BannedIds",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BannedIds");
        }
    }
}
