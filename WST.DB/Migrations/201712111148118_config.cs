namespace WST.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class config : DbMigration
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
                "dbo.Admin",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Account = c.String(maxLength: 32),
                        Password = c.String(maxLength: 32),
                        Sex = c.Int(nullable: false),
                        Mobile = c.String(maxLength: 32),
                        RealName = c.String(maxLength: 32),
                        RoleID = c.String(nullable: false, maxLength: 32),
                        IsSuperAdmin = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Adviser",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Sex = c.Int(nullable: false),
                        Mobile = c.String(maxLength: 32),
                        Name = c.String(maxLength: 32),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Carousel",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Title = c.String(nullable: false, maxLength: 32),
                        Url = c.String(maxLength: 256),
                        Picture = c.String(maxLength: 256),
                        Sort = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DataDictionary",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        ParentKey = c.String(maxLength: 32),
                        GroupCode = c.Int(nullable: false),
                        Key = c.String(maxLength: 32),
                        Value = c.String(nullable: false, maxLength: 32),
                        Remark = c.String(maxLength: 128),
                        Sort = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.KanJia",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        GoodsItemsJson = c.String(),
                        OldPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CountLimit = c.Int(nullable: false),
                        LessPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OncePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LimitHour = c.Int(nullable: false),
                        PrizeCount = c.Int(nullable: false),
                        UsedCount = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.Menu",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        Sort = c.Int(),
                        Link = c.String(maxLength: 64),
                        ParentID = c.String(maxLength: 32),
                        ClassName = c.String(maxLength: 32),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Operate",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        ActionUrl = c.String(nullable: false, maxLength: 64),
                        Sort = c.Int(),
                        Remark = c.String(maxLength: 64),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PayOrder",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        NO = c.String(nullable: false, maxLength: 32),
                        UserID = c.String(nullable: false, maxLength: 32),
                        ThirdOrderID = c.String(maxLength: 32),
                        Code = c.Int(nullable: false),
                        TargetID = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Remark = c.String(),
                        SuccessTime = c.DateTime(),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PinTuan",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        PinTuanItemJson = c.String(maxLength: 256),
                        PinTuanItemInfo = c.String(maxLength: 1024),
                        GoodsItemsJson = c.String(),
                        OldPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JoinCount = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.RechargePlan",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        Money = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Day = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        Remark = c.String(maxLength: 128, unicode: false),
                        MenuIDStr = c.String(unicode: false, storeType: "text"),
                        OperateStr = c.String(unicode: false, storeType: "text"),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Template",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        JsonData = c.String(),
                        CategoryID = c.String(nullable: false, maxLength: 32),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TemplateCategory",
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
                "dbo.User",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        OpenID = c.String(maxLength: 32),
                        HeadImgUrl = c.String(maxLength: 512),
                        Country = c.String(maxLength: 32),
                        Province = c.String(maxLength: 32),
                        City = c.String(maxLength: 32),
                        Sex = c.Int(nullable: false),
                        Mobile = c.String(maxLength: 11),
                        NickName = c.String(maxLength: 32),
                        IsMember = c.Boolean(nullable: false),
                        IDCard = c.String(),
                        StoreName = c.String(),
                        AdviserID = c.String(maxLength: 32),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalRecharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserActivity",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        JoinUserID = c.String(maxLength: 32),
                        Openid = c.String(nullable: false, maxLength: 32),
                        Code = c.Int(nullable: false),
                        TargetID = c.String(),
                        IsPrize = c.Boolean(nullable: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        PrizeInfo = c.String(),
                        SN = c.Int(nullable: false),
                        Mobile = c.String(maxLength: 11),
                        JoinUserName = c.String(nullable: false, maxLength: 32),
                        TargetUserID = c.String(maxLength: 11),
                        ShopUserID = c.String(nullable: false, maxLength: 32),
                        IsUsedOnLine = c.Boolean(nullable: false),
                        UsedTime = c.DateTime(),
                        FiledItemJson = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserWechat",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 32),
                        Name = c.String(nullable: false, maxLength: 32),
                        AppId = c.String(nullable: false),
                        AppSecrect = c.String(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserWechat");
            DropTable("dbo.UserActivity");
            DropTable("dbo.User");
            DropTable("dbo.TemplateCategory");
            DropTable("dbo.Template");
            DropTable("dbo.Role");
            DropTable("dbo.RechargePlan");
            DropTable("dbo.PinTuan");
            DropTable("dbo.PayOrder");
            DropTable("dbo.Operate");
            DropTable("dbo.Menu");
            DropTable("dbo.KanJia");
            DropTable("dbo.Demo");
            DropTable("dbo.DataDictionary");
            DropTable("dbo.Carousel");
            DropTable("dbo.Adviser");
            DropTable("dbo.Admin");
            DropTable("dbo.ActivityCategory");
        }
    }
}
