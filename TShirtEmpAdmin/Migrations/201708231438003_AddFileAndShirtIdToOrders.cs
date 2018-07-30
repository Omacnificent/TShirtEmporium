namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileAndShirtIdToOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ShirtId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "FileId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "FileId");
            DropColumn("dbo.Orders", "ShirtId");
        }
    }
}
