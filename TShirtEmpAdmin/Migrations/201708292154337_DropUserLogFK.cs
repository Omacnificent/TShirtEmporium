namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropUserLogFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserLogs", "Customer_Id", "dbo.AspNetUsers");
        }
        
        public override void Down()
        {
        }
    }
}
