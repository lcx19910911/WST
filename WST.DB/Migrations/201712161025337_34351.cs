namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _34351 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Music", "CategoryID", c => c.String(nullable: false, maxLength: 32));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Music", "CategoryID");
        }
    }
}
