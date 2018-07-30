namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shirts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shirts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TabName = c.String(),
                        ShirtCause = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Caption = c.String(),
                        SiteActive = c.Boolean(),
                        ShirtSizes_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShirtSizes", t => t.ShirtSizes_Id)
                .Index(t => t.ShirtSizes_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shirts", "ShirtSizes_Id", "dbo.ShirtSizes");
            DropIndex("dbo.Shirts", new[] { "ShirtSizes_Id" });
            DropTable("dbo.Shirts");
        }
    }
}
