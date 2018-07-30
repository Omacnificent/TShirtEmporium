namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrdersModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Position = c.String(),
                        PrefName = c.String(),
                        Company = c.String(),
                        CostCenter = c.String(),
                        DeptName = c.String(),
                        Email = c.String(),
                        Manager = c.String(),
                        VP = c.String(),
                        ShirtCause = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShirtYear = c.String(),
                        EmpId_Id = c.String(maxLength: 128),
                        ShirtQuantity_Id = c.Int(),
                        ShirtSize_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.EmpId_Id)
                .ForeignKey("dbo.ShirtQuantities", t => t.ShirtQuantity_Id)
                .ForeignKey("dbo.ShirtSizes", t => t.ShirtSize_Id)
                .Index(t => t.EmpId_Id)
                .Index(t => t.ShirtQuantity_Id)
                .Index(t => t.ShirtSize_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ShirtSize_Id", "dbo.ShirtSizes");
            DropForeignKey("dbo.Orders", "ShirtQuantity_Id", "dbo.ShirtQuantities");
            DropForeignKey("dbo.Orders", "EmpId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "ShirtSize_Id" });
            DropIndex("dbo.Orders", new[] { "ShirtQuantity_Id" });
            DropIndex("dbo.Orders", new[] { "EmpId_Id" });
            DropTable("dbo.Orders");
        }
    }
}
