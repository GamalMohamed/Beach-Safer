namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMyEntites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Beaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LongitudeBegin = c.String(),
                        LongitudeEnd = c.String(),
                        Latitude = c.String(),
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
                        Logo = c.String(),
                        SubscriptionType = c.String(),
                        JoinDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeviceLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceId = c.Int(),
                        Timestamp = c.DateTime(),
                        MessageId = c.Int(),
                        Status = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.DeviceId)
                .Index(t => t.DeviceId);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IsOwned = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceUsers", t => t.Id)
                .Index(t => t.Id);
            
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
                        Gender = c.String(),
                        SwimmingSkills = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devices", "Id", "dbo.DeviceUsers");
            DropForeignKey("dbo.DeviceLogs", "DeviceId", "dbo.Devices");
            DropForeignKey("dbo.Beaches", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Devices", new[] { "Id" });
            DropIndex("dbo.DeviceLogs", new[] { "DeviceId" });
            DropIndex("dbo.Beaches", new[] { "CustomerId" });
            DropTable("dbo.DeviceUsers");
            DropTable("dbo.Devices");
            DropTable("dbo.DeviceLogs");
            DropTable("dbo.Customers");
            DropTable("dbo.Beaches");
        }
    }
}
