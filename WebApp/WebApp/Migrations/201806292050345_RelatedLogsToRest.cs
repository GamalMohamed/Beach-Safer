namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelatedLogsToRest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceLogs", "CustomerId", c => c.Int());
            AddColumn("dbo.DeviceLogs", "BeachId", c => c.Int());
            AddColumn("dbo.DeviceLogs", "DeviceUserId", c => c.Int());
            CreateIndex("dbo.DeviceLogs", "CustomerId");
            CreateIndex("dbo.DeviceLogs", "BeachId");
            CreateIndex("dbo.DeviceLogs", "DeviceUserId");
            AddForeignKey("dbo.DeviceLogs", "BeachId", "dbo.Beaches", "Id");
            AddForeignKey("dbo.DeviceLogs", "CustomerId", "dbo.Customers", "Id");
            AddForeignKey("dbo.DeviceLogs", "DeviceUserId", "dbo.DeviceUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceLogs", "DeviceUserId", "dbo.DeviceUsers");
            DropForeignKey("dbo.DeviceLogs", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.DeviceLogs", "BeachId", "dbo.Beaches");
            DropIndex("dbo.DeviceLogs", new[] { "DeviceUserId" });
            DropIndex("dbo.DeviceLogs", new[] { "BeachId" });
            DropIndex("dbo.DeviceLogs", new[] { "CustomerId" });
            DropColumn("dbo.DeviceLogs", "DeviceUserId");
            DropColumn("dbo.DeviceLogs", "BeachId");
            DropColumn("dbo.DeviceLogs", "CustomerId");
        }
    }
}
