namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShirtQuantity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShirtQuantities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Shirts", "ShirtQuantities_Id", c => c.Int());
            CreateIndex("dbo.Shirts", "ShirtQuantities_Id");
            AddForeignKey("dbo.Shirts", "ShirtQuantities_Id", "dbo.ShirtQuantities", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shirts", "ShirtQuantities_Id", "dbo.ShirtQuantities");
            DropIndex("dbo.Shirts", new[] { "ShirtQuantities_Id" });
            DropColumn("dbo.Shirts", "ShirtQuantities_Id");
            DropTable("dbo.ShirtQuantities");
        }
    }
}
