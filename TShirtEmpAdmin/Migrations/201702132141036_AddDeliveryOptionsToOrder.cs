namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeliveryOptionsToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ODeliveryOption", c => c.String());
            AddColumn("dbo.Orders", "DeliveryOption_Id", c => c.Int());
            CreateIndex("dbo.Orders", "DeliveryOption_Id");
            AddForeignKey("dbo.Orders", "DeliveryOption_Id", "dbo.DeliveryOptions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "DeliveryOption_Id", "dbo.DeliveryOptions");
            DropIndex("dbo.Orders", new[] { "DeliveryOption_Id" });
            DropColumn("dbo.Orders", "DeliveryOption_Id");
            DropColumn("dbo.Orders", "ODeliveryOption");
        }
    }
}
