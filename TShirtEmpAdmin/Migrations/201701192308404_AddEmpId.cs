namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmpId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EmployeeId");
        }
    }
}
