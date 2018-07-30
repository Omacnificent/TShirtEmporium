namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderSizeandQuantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OShirtSize", c => c.String());
            AddColumn("dbo.Orders", "OShirtQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OShirtQuantity");
            DropColumn("dbo.Orders", "OShirtSize");
        }
    }
}
