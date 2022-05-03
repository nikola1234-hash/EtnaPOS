namespace EtnaPOS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMIgration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artikals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
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
                        CreatedBy = c.String(),
                        DateCreated = c.DateTime(nullable: false),
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
            DropForeignKey("dbo.Artikals", "KategorijaArtiklaId", "dbo.KategorijaArtiklas");
            DropIndex("dbo.Artikals", new[] { "KategorijaArtiklaId" });
            DropTable("dbo.Users");
            DropTable("dbo.KategorijaArtiklas");
            DropTable("dbo.Artikals");
        }
    }
}
