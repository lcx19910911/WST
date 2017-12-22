namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _34365 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Password", c => c.String(maxLength: 32));
            DropColumn("dbo.User", "IDCard");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "IDCard", c => c.String());
            DropColumn("dbo.User", "Password");
        }
    }
}
