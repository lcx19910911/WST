namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _89ii : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Music",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        Url = c.String(maxLength: 256),
                        Sort = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Music");
        }
    }
}
