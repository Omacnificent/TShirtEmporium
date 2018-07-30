namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpsizeChargeToShirt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shirts", "UpsizeCharge", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shirts", "UpsizeCharge");
        }
    }
}
