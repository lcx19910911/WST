namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _343dd : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserActivity", "SN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserActivity", "SN", c => c.Int(nullable: false));
        }
    }
}
