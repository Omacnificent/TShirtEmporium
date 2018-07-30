namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShirtToSize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShirtToSizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Shirt_Id = c.Int(),
                        ShirtSize_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shirts", t => t.Shirt_Id)
                .ForeignKey("dbo.ShirtSizes", t => t.ShirtSize_Id)
                .Index(t => t.Shirt_Id)
                .Index(t => t.ShirtSize_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShirtToSizes", "ShirtSize_Id", "dbo.ShirtSizes");
            DropForeignKey("dbo.ShirtToSizes", "Shirt_Id", "dbo.Shirts");
            DropIndex("dbo.ShirtToSizes", new[] { "ShirtSize_Id" });
            DropIndex("dbo.ShirtToSizes", new[] { "Shirt_Id" });
            DropTable("dbo.ShirtToSizes");
        }
    }
}
