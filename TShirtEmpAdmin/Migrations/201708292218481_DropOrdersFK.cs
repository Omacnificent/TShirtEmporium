namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropOrdersFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.AspNetUsers");
        }
        
        public override void Down()
        {
        }
    }
}

