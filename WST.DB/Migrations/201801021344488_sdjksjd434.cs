namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdjksjd434 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PinTu",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        PinTuItemJson = c.String(maxLength: 256),
                        JoinCount = c.Int(nullable: false),
                        LessPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TemplateUrl = c.String(maxLength: 32),
                        OldPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        PrizeCount = c.Int(nullable: false),
                        UsedCount = c.Int(nullable: false),
                        GoodsItemsJson = c.String(),
                        UserID = c.String(nullable: false, maxLength: 32),
                        StaticHtmlUrl = c.String(maxLength: 256),
                        Name = c.String(maxLength: 32),
                        Picture = c.String(nullable: false, maxLength: 256),
                        IsCompalte = c.Boolean(nullable: false),
                        IsNeedPay = c.Boolean(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsNeedReport = c.Boolean(nullable: false),
                        IntroduceTxtJson = c.String(nullable: false),
                        IntroducePicturesJson = c.String(maxLength: 1024),
                        IntroduceVediosJson = c.String(maxLength: 1024),
                        Connact = c.String(nullable: false, maxLength: 1024),
                        PhoneNumber = c.String(nullable: false, maxLength: 11),
                        Rules = c.String(nullable: false, maxLength: 1024),
                        MusicUrl = c.String(maxLength: 128),
                        FunctionName = c.String(maxLength: 32),
                        Direction = c.String(maxLength: 32),
                        ShareCount = c.Int(nullable: false),
                        ReportCount = c.Int(nullable: false),
                        ClickCount = c.Int(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        FiledItemJson = c.String(maxLength: 512),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PinTu");
        }
    }
}
