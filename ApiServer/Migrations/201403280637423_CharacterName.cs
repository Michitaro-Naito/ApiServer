namespace ApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CharacterName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Characters", "Name", c => c.String(nullable: false, maxLength: 12));
            CreateIndex("dbo.Characters", "Name", true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Characters", "Name");
            AlterColumn("dbo.Characters", "Name", c => c.String());
        }
    }
}
