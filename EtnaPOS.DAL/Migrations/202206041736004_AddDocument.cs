namespace EtnaPOS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocument : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TableId = c.Int(nullable: false),
                        IsOpen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tables", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);
            
            AddColumn("dbo.Orders", "Document_Id", c => c.Guid());
            CreateIndex("dbo.Orders", "Document_Id");
            AddForeignKey("dbo.Orders", "Document_Id", "dbo.Documents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "TableId", "dbo.Tables");
            DropForeignKey("dbo.Orders", "Document_Id", "dbo.Documents");
            DropIndex("dbo.Orders", new[] { "Document_Id" });
            DropIndex("dbo.Documents", new[] { "TableId" });
            DropColumn("dbo.Orders", "Document_Id");
            DropTable("dbo.Documents");
        }
    }
}
