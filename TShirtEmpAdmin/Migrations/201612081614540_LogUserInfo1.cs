namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogUserInfo1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserInfo_Employee", "EmpID", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInfo_Employee", "EmpID", c => c.Single(nullable: false));
        }
    }
}
