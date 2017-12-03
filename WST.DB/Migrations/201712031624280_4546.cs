namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4546 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityCategory",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        Sort = c.Int(),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Demo",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        Description = c.String(maxLength: 128),
                        Content = c.String(),
                        Picture = c.String(maxLength: 256),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.User", "NickName", c => c.String(maxLength: 32));
            AddColumn("dbo.User", "StoreName", c => c.String());
            DropColumn("dbo.Role", "Discount");
            DropColumn("dbo.User", "RealName");
            DropColumn("dbo.User", "Discount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Discount", c => c.Int(nullable: false));
            AddColumn("dbo.User", "RealName", c => c.String(maxLength: 32));
            AddColumn("dbo.Role", "Discount", c => c.Int(nullable: false));
            DropColumn("dbo.User", "StoreName");
            DropColumn("dbo.User", "NickName");
            DropTable("dbo.Demo");
            DropTable("dbo.ActivityCategory");
        }
    }
}
