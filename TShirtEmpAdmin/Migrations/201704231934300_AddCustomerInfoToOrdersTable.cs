namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerInfoToOrdersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "EmployeeID", c => c.String());
            AddColumn("dbo.Orders", "FirstName", c => c.String());
            AddColumn("dbo.Orders", "LastName", c => c.String());
            AddColumn("dbo.Orders", "Department", c => c.String());
            AddColumn("dbo.Orders", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PhoneNumber");
            DropColumn("dbo.Orders", "Department");
            DropColumn("dbo.Orders", "LastName");
            DropColumn("dbo.Orders", "FirstName");
            DropColumn("dbo.Orders", "EmployeeID");
        }
    }
}
