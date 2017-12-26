namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dsd1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Template", "TemplateUrl", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Template", "TemplateUrl");
        }
    }
}
