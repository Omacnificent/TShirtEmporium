namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogDate = c.DateTime(nullable: false),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            AddColumn("dbo.Orders", "OrderNum", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLogs", "Customer_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserLogs", new[] { "Customer_Id" });
            DropColumn("dbo.Orders", "OrderNum");
            DropTable("dbo.UserLogs");
        }
    }
}
