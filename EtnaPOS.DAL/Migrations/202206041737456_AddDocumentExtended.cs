namespace EtnaPOS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentExtended : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "Date", c => c.DateTime(nullable: false, defaultValue: DateTime.Now));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "Date");
        }
    }
}
