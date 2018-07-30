namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShirtToSizeRemoveId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ShirtToSizes");
            AddPrimaryKey("dbo.ShirtToSizes", new[] { "ShirtId", "ShirtSizeId" });
            CreateIndex("dbo.ShirtToSizes", "ShirtId");
            CreateIndex("dbo.ShirtToSizes", "ShirtSizeId");
            AddForeignKey("dbo.ShirtToSizes", "ShirtId", "dbo.Shirts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ShirtToSizes", "ShirtSizeId", "dbo.ShirtSizes", "Id", cascadeDelete: true);
            DropColumn("dbo.ShirtToSizes", "Id");
            DropColumn("dbo.ShirtToSizes", "IsSelected");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShirtToSizes", "IsSelected", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShirtToSizes", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.ShirtToSizes", "ShirtSizeId", "dbo.ShirtSizes");
            DropForeignKey("dbo.ShirtToSizes", "ShirtId", "dbo.Shirts");
            DropIndex("dbo.ShirtToSizes", new[] { "ShirtSizeId" });
            DropIndex("dbo.ShirtToSizes", new[] { "ShirtId" });
            DropPrimaryKey("dbo.ShirtToSizes");
            AddPrimaryKey("dbo.ShirtToSizes", "Id");
        }
    }
}
