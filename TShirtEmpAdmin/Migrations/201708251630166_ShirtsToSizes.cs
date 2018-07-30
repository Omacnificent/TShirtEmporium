namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShirtsToSizes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShirtToSizes", "Shirt_Id", "dbo.Shirts");
            DropForeignKey("dbo.ShirtToSizes", "ShirtSize_Id", "dbo.ShirtSizes");
            DropIndex("dbo.ShirtToSizes", new[] { "Shirt_Id" });
            DropIndex("dbo.ShirtToSizes", new[] { "ShirtSize_Id" });
            AddColumn("dbo.ShirtToSizes", "ShirtId", c => c.Int(nullable: false));
            AddColumn("dbo.ShirtToSizes", "ShirtSizeId", c => c.Int(nullable: false));
            DropColumn("dbo.ShirtToSizes", "Shirt_Id");
            DropColumn("dbo.ShirtToSizes", "ShirtSize_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShirtToSizes", "ShirtSize_Id", c => c.Int());
            AddColumn("dbo.ShirtToSizes", "Shirt_Id", c => c.Int());
            DropColumn("dbo.ShirtToSizes", "ShirtSizeId");
            DropColumn("dbo.ShirtToSizes", "ShirtId");
            CreateIndex("dbo.ShirtToSizes", "ShirtSize_Id");
            CreateIndex("dbo.ShirtToSizes", "Shirt_Id");
            AddForeignKey("dbo.ShirtToSizes", "ShirtSize_Id", "dbo.ShirtSizes", "Id");
            AddForeignKey("dbo.ShirtToSizes", "Shirt_Id", "dbo.Shirts", "Id");
        }
    }
}
