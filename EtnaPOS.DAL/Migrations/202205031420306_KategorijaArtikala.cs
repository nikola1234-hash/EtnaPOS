namespace EtnaPOS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KategorijaArtikala : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KategorijaArtiklas", "ParentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KategorijaArtiklas", "ParentId");
        }
    }
}
