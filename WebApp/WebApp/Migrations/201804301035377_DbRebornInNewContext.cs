namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbRebornInNewContext : DbMigration
    {
        public override void Up()
        {
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
            
            CreateTable(
                "dbo.Beaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StartPoint = c.String(),
                        EndPoint = c.String(),
                        LastSeaPoint = c.String(),
                        CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(),
                        Logo = c.String(),
                        Subscription = c.Int(nullable: false),
                        JoinDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceState = c.Int(nullable: false),
                        DeviceType = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        DeviceUserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.DeviceLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceId = c.Int(),
                        Timestamp = c.DateTime(),
                        MessageId = c.Int(),
                        State = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.DeviceId)
                .Index(t => t.DeviceId);
            
            CreateTable(
                "dbo.DeviceUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ProfilePic = c.String(),
                        Age = c.Int(),
                        Email = c.String(),
                        Phone = c.String(),
                        EmergencyPhone = c.String(),
                        Gender = c.String(),
                        SwimmingSkills = c.String(),
                        Notes = c.String(),
                        DeviceId = c.Int(nullable: false),
                        CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.UserTokenCaches",
                c => new
                    {
                        UserTokenCacheId = c.Int(nullable: false, identity: true),
                        webUserUniqueId = c.String(),
                        cacheBits = c.Binary(),
                        LastWrite = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserTokenCacheId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BeachAccesses", "Id", "dbo.Beaches");
            DropForeignKey("dbo.DeviceUsers", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.DeviceLogs", "DeviceId", "dbo.Devices");
            DropForeignKey("dbo.Devices", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Beaches", "CustomerId", "dbo.Customers");
            DropIndex("dbo.DeviceUsers", new[] { "CustomerId" });
            DropIndex("dbo.DeviceLogs", new[] { "DeviceId" });
            DropIndex("dbo.Devices", new[] { "CustomerId" });
            DropIndex("dbo.Beaches", new[] { "CustomerId" });
            DropIndex("dbo.BeachAccesses", new[] { "Id" });
            DropTable("dbo.UserTokenCaches");
            DropTable("dbo.DeviceUsers");
            DropTable("dbo.DeviceLogs");
            DropTable("dbo.Devices");
            DropTable("dbo.Customers");
            DropTable("dbo.Beaches");
            DropTable("dbo.BeachAccesses");
        }
    }
}
