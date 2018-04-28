namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RedefiningRelations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeviceLogs", "DeviceId", "dbo.Devices");
            DropForeignKey("dbo.Devices", "Id", "dbo.DeviceUsers");
            DropIndex("dbo.Devices", new[] { "Id" });
            DropPrimaryKey("dbo.Devices");
            DropPrimaryKey("dbo.DeviceUsers");
            CreateTable(
                "dbo.CustomerAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AccessCode = c.String(),
                        AccessCodeHash = c.String(),
                        CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Customers", "CustomerAccessId", c => c.Int());
            AddColumn("dbo.Devices", "CustomerId", c => c.Int());
            AddColumn("dbo.DeviceUsers", "DeviceId", c => c.Int(nullable: false));
            AddColumn("dbo.DeviceUsers", "CustomerId", c => c.Int());
            AlterColumn("dbo.Devices", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.DeviceUsers", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Devices", "Id");
            AddPrimaryKey("dbo.DeviceUsers", "Id");
            CreateIndex("dbo.Devices", "CustomerId");
            CreateIndex("dbo.DeviceUsers", "Id");
            CreateIndex("dbo.DeviceUsers", "CustomerId");
            AddForeignKey("dbo.Devices", "CustomerId", "dbo.Customers", "Id");
            AddForeignKey("dbo.DeviceUsers", "CustomerId", "dbo.Customers", "Id");
            AddForeignKey("dbo.DeviceLogs", "DeviceId", "dbo.Devices", "Id");
            AddForeignKey("dbo.DeviceUsers", "Id", "dbo.Devices", "Id");
            DropColumn("dbo.Customers", "AccessCode");
            DropColumn("dbo.Customers", "AccessCodeHash");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "AccessCodeHash", c => c.String());
            AddColumn("dbo.Customers", "AccessCode", c => c.String());
            DropForeignKey("dbo.DeviceUsers", "Id", "dbo.Devices");
            DropForeignKey("dbo.DeviceLogs", "DeviceId", "dbo.Devices");
            DropForeignKey("dbo.DeviceUsers", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Devices", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerAccesses", "Id", "dbo.Customers");
            DropIndex("dbo.DeviceUsers", new[] { "CustomerId" });
            DropIndex("dbo.DeviceUsers", new[] { "Id" });
            DropIndex("dbo.Devices", new[] { "CustomerId" });
            DropIndex("dbo.CustomerAccesses", new[] { "Id" });
            DropPrimaryKey("dbo.DeviceUsers");
            DropPrimaryKey("dbo.Devices");
            AlterColumn("dbo.DeviceUsers", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Devices", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.DeviceUsers", "CustomerId");
            DropColumn("dbo.DeviceUsers", "DeviceId");
            DropColumn("dbo.Devices", "CustomerId");
            DropColumn("dbo.Customers", "CustomerAccessId");
            DropTable("dbo.CustomerAccesses");
            AddPrimaryKey("dbo.DeviceUsers", "Id");
            AddPrimaryKey("dbo.Devices", "Id");
            CreateIndex("dbo.Devices", "Id");
            AddForeignKey("dbo.Devices", "Id", "dbo.DeviceUsers", "Id");
            AddForeignKey("dbo.DeviceLogs", "DeviceId", "dbo.Devices", "Id");
        }
    }
}
