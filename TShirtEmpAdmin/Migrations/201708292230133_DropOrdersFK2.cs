namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropOrdersFK2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "EmpId_Id", "dbo.AspNetUsers");
        }
        
        public override void Down()
        {
        }
    }
}
