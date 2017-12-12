namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _34331212 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KanJia", "PhoneNumber", c => c.String(nullable: false, maxLength: 11));
            AddColumn("dbo.PinTuan", "PhoneNumber", c => c.String(nullable: false, maxLength: 11));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PinTuan", "PhoneNumber");
            DropColumn("dbo.KanJia", "PhoneNumber");
        }
    }
}
