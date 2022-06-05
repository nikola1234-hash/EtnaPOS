namespace EtnaPOS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ArtikalId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false, defaultValue: DateTime.Now),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artikals", t => t.ArtikalId, cascadeDelete: true)
                .Index(t => t.ArtikalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ArtikalId", "dbo.Artikals");
            DropIndex("dbo.Orders", new[] { "ArtikalId" });
            DropTable("dbo.Orders");
        }
    }
}
