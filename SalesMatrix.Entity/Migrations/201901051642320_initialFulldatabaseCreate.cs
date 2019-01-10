namespace SalesMatrix.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialFulldatabaseCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DiscountTitle = c.String(),
                        Description = c.String(),
                        ItemId = c.Int(),
                        CategoryId = c.Int(),
                        DiscountRate = c.Double(nullable: false),
                        DiscountAmount = c.Double(nullable: false),
                        DiscountStartDate = c.DateTime(),
                        DiscountEndDate = c.DateTime(),
                        IsContinue = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedFrom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        Description = c.String(),
                        ParentItemId = c.Int(),
                        DisplaySequence = c.Int(nullable: false),
                        RecorderLevel = c.Int(nullable: false),
                        IsActualItem = c.Boolean(nullable: false),
                        Picture = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedFrom = c.String(),
                        Discount_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Discounts", t => t.Discount_Id)
                .ForeignKey("dbo.Items", t => t.ParentItemId)
                .Index(t => t.ParentItemId)
                .Index(t => t.Discount_Id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleName = c.String(),
                        Description = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedFrom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Operations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OperationName = c.String(),
                        Description = c.String(),
                        IsGlobal = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedFrom = c.String(),
                        Status = c.Boolean(nullable: false),
                        ModuleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PermissionType = c.String(),
                        Name = c.String(),
                        IsCreate = c.Boolean(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        IsEdit = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        IsPrint = c.Boolean(nullable: false),
                        IsExclusive = c.Boolean(nullable: false),
                        RoleId = c.Int(),
                        UserId = c.Int(),
                        ResourceId = c.Int(),
                        OperationId = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedFrom = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Operations", t => t.OperationId)
                .ForeignKey("dbo.Resources", t => t.ResourceId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.ResourceId)
                .Index(t => t.OperationId);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourceType = c.String(),
                        ResourceName = c.String(),
                        Description = c.String(),
                        Parent = c.Int(),
                        Sequence = c.Int(nullable: false),
                        IsGlobal = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedFrom = c.String(),
                        Status = c.Boolean(nullable: false),
                        ModuleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.Resources", t => t.Parent)
                .Index(t => t.Parent)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        Description = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedFrom = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FullName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Picture = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedFrom = c.String(),
                        Status = c.Boolean(nullable: false),
                        RoleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RetailCustomers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        Address = c.String(),
                        Region = c.String(),
                        MobileNo = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedFrom = c.String(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        ModifiedFrom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Permissions", "UserId", "dbo.Users");
            DropForeignKey("dbo.Permissions", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Permissions", "ResourceId", "dbo.Resources");
            DropForeignKey("dbo.Resources", "Parent", "dbo.Resources");
            DropForeignKey("dbo.Resources", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Permissions", "OperationId", "dbo.Operations");
            DropForeignKey("dbo.Operations", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Items", "ParentItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "Discount_Id", "dbo.Discounts");
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Resources", new[] { "ModuleId" });
            DropIndex("dbo.Resources", new[] { "Parent" });
            DropIndex("dbo.Permissions", new[] { "OperationId" });
            DropIndex("dbo.Permissions", new[] { "ResourceId" });
            DropIndex("dbo.Permissions", new[] { "UserId" });
            DropIndex("dbo.Permissions", new[] { "RoleId" });
            DropIndex("dbo.Operations", new[] { "ModuleId" });
            DropIndex("dbo.Items", new[] { "Discount_Id" });
            DropIndex("dbo.Items", new[] { "ParentItemId" });
            DropTable("dbo.RetailCustomers");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Resources");
            DropTable("dbo.Permissions");
            DropTable("dbo.Operations");
            DropTable("dbo.Modules");
            DropTable("dbo.Items");
            DropTable("dbo.Discounts");
        }
    }
}
