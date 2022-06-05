namespace EtnaPOS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class extendedOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Time", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsDeleted");
            DropColumn("dbo.Orders", "Time");
        }
    }
}
