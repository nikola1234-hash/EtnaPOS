namespace EtnaPOS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documentExtending : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "TotalPrice");
        }
    }
}
