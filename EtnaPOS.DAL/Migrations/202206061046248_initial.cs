namespace EtnaPOS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artikals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        KategorijaArtiklaId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KategorijaArtiklas", t => t.KategorijaArtiklaId, cascadeDelete: true)
                .Index(t => t.KategorijaArtiklaId);
            
            CreateTable(
                "dbo.KategorijaArtiklas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Kategorija = c.String(),
                        ParentId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TableId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Time = c.DateTime(nullable: false),
                        IsOpen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tables", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ArtikalId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Time = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Document_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artikals", t => t.ArtikalId, cascadeDelete: true)
                .ForeignKey("dbo.Documents", t => t.Document_Id)
                .Index(t => t.ArtikalId)
                .Index(t => t.Document_Id);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                        CreatedBy = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "TableId", "dbo.Tables");
            DropForeignKey("dbo.Orders", "Document_Id", "dbo.Documents");
            DropForeignKey("dbo.Orders", "ArtikalId", "dbo.Artikals");
            DropForeignKey("dbo.Artikals", "KategorijaArtiklaId", "dbo.KategorijaArtiklas");
            DropIndex("dbo.Orders", new[] { "Document_Id" });
            DropIndex("dbo.Orders", new[] { "ArtikalId" });
            DropIndex("dbo.Documents", new[] { "TableId" });
            DropIndex("dbo.Artikals", new[] { "KategorijaArtiklaId" });
            DropTable("dbo.Users");
            DropTable("dbo.Tables");
            DropTable("dbo.Orders");
            DropTable("dbo.Documents");
            DropTable("dbo.KategorijaArtiklas");
            DropTable("dbo.Artikals");
        }
    }
}
