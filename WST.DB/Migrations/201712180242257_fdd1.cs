namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fdd1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RechargePlan", "Introduce", c => c.String(nullable: false, maxLength: 32));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RechargePlan", "Introduce");
        }
    }
}
