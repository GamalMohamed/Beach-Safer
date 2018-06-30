namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedSomeRedundantRelations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeviceLogs", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.DeviceLogs", "DeviceUserId", "dbo.DeviceUsers");
            DropIndex("dbo.DeviceLogs", new[] { "CustomerId" });
            DropIndex("dbo.DeviceLogs", new[] { "DeviceUserId" });
            DropColumn("dbo.DeviceLogs", "CustomerId");
            DropColumn("dbo.DeviceLogs", "DeviceUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeviceLogs", "DeviceUserId", c => c.Int());
            AddColumn("dbo.DeviceLogs", "CustomerId", c => c.Int());
            CreateIndex("dbo.DeviceLogs", "DeviceUserId");
            CreateIndex("dbo.DeviceLogs", "CustomerId");
            AddForeignKey("dbo.DeviceLogs", "DeviceUserId", "dbo.DeviceUsers", "Id");
            AddForeignKey("dbo.DeviceLogs", "CustomerId", "dbo.Customers", "Id");
        }
    }
}
