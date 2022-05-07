namespace EtnaPOS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class artikalDecimalChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Artikals", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Artikals", "Price", c => c.Double(nullable: false));
        }
    }
}
