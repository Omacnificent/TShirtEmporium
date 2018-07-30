namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShirtToSizeSelected : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShirtToSizes", "IsSelected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShirtToSizes", "IsSelected");
        }
    }
}
