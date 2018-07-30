namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailToOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Email");
        }
    }
}
