namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeShirtQuantityToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShirtQuantities", "Name", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShirtQuantities", "Name", c => c.String());
        }
    }
}
