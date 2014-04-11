namespace ApiServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Character_Items : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "JsonItems", c => c.String(nullable: false));
            AddColumn("dbo.Characters", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Characters", "RowVersion");
            DropColumn("dbo.Characters", "JsonItems");
        }
    }
}
