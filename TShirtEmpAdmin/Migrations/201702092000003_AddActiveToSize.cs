namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveToSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShirtSizes", "Active", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShirtSizes", "Active");
        }
    }
}
