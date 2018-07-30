namespace TShirtEmpAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Files : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        ShirtId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.Shirts", t => t.ShirtId, cascadeDelete: true)
                .Index(t => t.ShirtId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "ShirtId", "dbo.Shirts");
            DropIndex("dbo.Files", new[] { "ShirtId" });
            DropTable("dbo.Files");
        }
    }
}
