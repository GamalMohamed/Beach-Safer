namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReconfiguredAccessCode : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerAccesses", "Id", "dbo.Customers");
            DropIndex("dbo.CustomerAccesses", new[] { "Id" });
            CreateTable(
                "dbo.BeachAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AccessCode = c.String(),
                        AccessCodeHash = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Beaches", t => t.Id)
                .Index(t => t.Id);
            
            DropTable("dbo.CustomerAccesses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CustomerAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AccessCode = c.String(),
                        AccessCodeHash = c.String(),
                        CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.BeachAccesses", "Id", "dbo.Beaches");
            DropIndex("dbo.BeachAccesses", new[] { "Id" });
            DropTable("dbo.BeachAccesses");
            CreateIndex("dbo.CustomerAccesses", "Id");
            AddForeignKey("dbo.CustomerAccesses", "Id", "dbo.Customers", "Id");
        }
    }
}
