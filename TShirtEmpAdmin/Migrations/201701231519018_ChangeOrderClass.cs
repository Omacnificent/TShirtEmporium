namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeOrderClass : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Orders", name: "EmpId_Id", newName: "Customer_Id");
            RenameIndex(table: "dbo.Orders", name: "IX_EmpId_Id", newName: "IX_Customer_Id");
            AddColumn("dbo.Orders", "OrderCompleted", c => c.Boolean());
            AddColumn("dbo.Orders", "RecievedShirt", c => c.Boolean());
            DropColumn("dbo.Orders", "FirstName");
            DropColumn("dbo.Orders", "LastName");
            DropColumn("dbo.Orders", "Position");
            DropColumn("dbo.Orders", "PrefName");
            DropColumn("dbo.Orders", "Company");
            DropColumn("dbo.Orders", "CostCenter");
            DropColumn("dbo.Orders", "DeptName");
            DropColumn("dbo.Orders", "Email");
            DropColumn("dbo.Orders", "Manager");
            DropColumn("dbo.Orders", "VP");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "VP", c => c.String());
            AddColumn("dbo.Orders", "Manager", c => c.String());
            AddColumn("dbo.Orders", "Email", c => c.String());
            AddColumn("dbo.Orders", "DeptName", c => c.String());
            AddColumn("dbo.Orders", "CostCenter", c => c.String());
            AddColumn("dbo.Orders", "Company", c => c.String());
            AddColumn("dbo.Orders", "PrefName", c => c.String());
            AddColumn("dbo.Orders", "Position", c => c.String());
            AddColumn("dbo.Orders", "LastName", c => c.String());
            AddColumn("dbo.Orders", "FirstName", c => c.String());
            DropColumn("dbo.Orders", "RecievedShirt");
            DropColumn("dbo.Orders", "OrderCompleted");
            RenameIndex(table: "dbo.Orders", name: "IX_Customer_Id", newName: "IX_EmpId_Id");
            RenameColumn(table: "dbo.Orders", name: "Customer_Id", newName: "EmpId_Id");
        }
    }
}
