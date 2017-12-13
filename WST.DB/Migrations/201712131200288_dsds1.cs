namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dsds1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserActivity", "TargetUserID", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserActivity", "TargetUserID", c => c.String(maxLength: 11));
        }
    }
}
