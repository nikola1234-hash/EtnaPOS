namespace EtnaPOS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documentsConected : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "ZatvaranjeDana_Id", c => c.Int());
            CreateIndex("dbo.Documents", "ZatvaranjeDana_Id");
            AddForeignKey("dbo.Documents", "ZatvaranjeDana_Id", "dbo.ZatvaranjeDanas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "ZatvaranjeDana_Id", "dbo.ZatvaranjeDanas");
            DropIndex("dbo.Documents", new[] { "ZatvaranjeDana_Id" });
            DropColumn("dbo.Documents", "ZatvaranjeDana_Id");
        }
    }
}
