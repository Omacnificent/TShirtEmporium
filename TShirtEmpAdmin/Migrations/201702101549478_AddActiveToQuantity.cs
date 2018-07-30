namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveToQuantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShirtQuantities", "Active", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShirtQuantities", "Active");
        }
    }
}
